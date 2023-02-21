using UnityEngine;

public class FitScreen : ScreenAdaptive
{
    public Vector2 sizeInPixels = new Vector2(340, 150);
    public float aspectRatio = 9 / 16f;
    public float screenAspectRatio = 9 / 16f;
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
        onResolutionChange(new Vector2Int(Screen.width, Screen.height));
    }

    public override void onResolutionChange(Vector2Int res)
    {
        var ratio = aspectRatio;
        var screenVector = new Vector2(res.x, res.y);
        var screenRatio = screenAspectRatio = screenVector.aspectRatio();

        var product = ratio * screenRatio;

        product *= .5f;
        var v = product * Vector3.one;


        transform.localScale = v;
    }
}