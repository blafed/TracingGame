using DG.Tweening;
using System.Collections.Generic;
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