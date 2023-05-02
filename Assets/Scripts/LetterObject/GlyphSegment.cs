using UnityEngine;
using System.Collections.Generic;
using System.Collections;
[System.Serializable]
public class GlyphSegment
{
    public Vector2 offset => _offset;
    public Path path => _path;
    public float pathLegnth => _pathLegnth;
    public bool isDot => _isDot;



    Vector2 _offset;
    Path _path;
    float _pathLegnth;
    bool _isDot;



    public GlyphSegment(Path path, bool isDot, Vector2 offset)
    {
        this._path = path;
        this._isDot = isDot;
        this._offset = offset;
        this._pathLegnth = path.totalLength;
    }
}