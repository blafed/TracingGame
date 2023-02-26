using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;


public static class TweenHelper
{
    public static Tween DOMoveCurvy(this Transform t, Vector2 point, float duration, float curveHeight = 1, Ease ease = Ease.Linear)
    {
        var origin = (Vector2)t.transform.position;
        var diff = point - origin;
        var mid = point + diff * .5f;
        mid += mid.getNormal().normalized * curveHeight;


        float f = 0;

        var tweenF = DOTween.To(() => f, x =>
        {
            f = x;
            t.position = BezierHelper.EvaluateQuadratic(origin, mid, point, f);
        }
        , 1, duration).SetEase(ease);
        return tweenF;
    }
}
public static class TweenOrganizer
{

    public static Tween add(this Tween tween, List<Tween> list)
    {
        list.Add(tween);
        return tween;
    }

    static List<Tween> tweens = new List<Tween>();
    static bool autoKill = true;
    public static void beginCollection(bool autoKill = true)
    {
        TweenOrganizer.autoKill = autoKill;
        tweens.Clear();
    }
    public static Tween add(this Tween t)
    {
        tweens.Add(t);
        return t;
    }
    public static void kill()
    {
        foreach (var x in tweens)
        {
            x.Kill();
        }
    }
    public static void rewind()
    {
        foreach (var x in tweens)
        {
            x.Rewind();
            x.Kill();
        }
    }
    public static List<Tween> getList() => tweens;
}