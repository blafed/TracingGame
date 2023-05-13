using System.Collections.Generic;
using UnityEngine;

using System;
using System.Collections;

[System.Serializable]

public class Glyph : IEnumerable<GlyphSegment>
{


    public float totalLength
    {
        get
        {
            if (_totalLength == null)
            {
                foreach (var x in segments)
                {
                    _totalLength += x.pathLegnth;
                }
            }
            return _totalLength.Value;
        }
    }



    public int segmentCount => segments.Count;
    public Rect relativeRect { get; private set; }


    float? _totalLength;

    [SerializeField]
    List<GlyphSegment> segments;

    //idle contrsuctor for unity serialization
    Glyph() { }
    public Glyph(List<GlyphSegment> segments, Rect relativeRect)
    {
        this.relativeRect = relativeRect;
        this.segments = segments;
        _totalLength = 0;
        foreach (var x in segments)
        {
            _totalLength += x.pathLegnth;
        }
    }


    public GlyphSegment get(int index)
    {
        return segments[index];
    }

    IEnumerator<GlyphSegment> IEnumerable<GlyphSegment>.GetEnumerator()
    {
        return segments.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return segments.GetEnumerator();
    }
}