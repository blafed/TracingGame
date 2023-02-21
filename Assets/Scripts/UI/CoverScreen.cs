using UnityEngine;
using UnityEngine.UI;
public class CoverScreen : ScreenAdaptive
{
    public float factor = 1;
    private void Awake()
    {
    }
#if UNITY_EDITOR
    private void Update()
    {
        OnEnable();
    }
#endif
    private void OnEnable()
    {
        onResolutionChange(Extensions2.screenVector.toInt());
    }

    public override void onResolutionChange(Vector2Int res)
    {
        Image image = GetComponent<Image>();
        var s = image.sprite;


        var ratio = s.texture.size().toVector2().aspectRatio();
        var currentRatio = res.x / (float)res.y;

        var error = currentRatio / ratio;
        if (currentRatio < ratio)
            error = 1 / error;


        var v = Vector3.one * error;
        transform.localScale = v * factor;
    }
}
