using DG.Tweening;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    [SerializeField] float shakeDuration = .1f;
    [SerializeField] float shakeStrength = 10f;


    public static CameraControl o;

    Transform rotor;

    Camera _camera;

    public new Camera camera => this.cachedComponentInChild(ref _camera);

    private void Awake()
    {
        o = this;
        rotor = transform.GetChild(0);
    }

    public void stop()
    {
        transform.DOKill();
    }

    public Tween move(Vector2 point, float duration = .5f, Ease ease = Ease.OutQuad)
    {
        return transform.DOMove(point.toVector3(transform.position.z), duration).SetEase(ease);
    }
    public Tween zoom(float orthoSize, float duration = 1, Ease ease = Ease.InQuad)
    {
        return camera.DOOrthoSize(orthoSize, duration).SetEase(ease);
    }

    public Tween punch(float punch = .2f, float duration = .2f)
    {
        return DOTween.Punch(() => new Vector3(camera.orthographicSize, 1, 1), x => camera.orthographicSize = x.x, punch.vector(), duration);
    }

    public Tween shake()
    {
        return camera.DOShakeRotation(shakeDuration, shakeStrength);
    }
}