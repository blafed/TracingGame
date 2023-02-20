// using System.Collections.Generic;
// using UnityEngine;

// [System.Serializable]
// public class Curve
// {
//     public static Vector2 quadraticCurve(Vector2 a, Vector2 b, Vector2 c, float t)
//     {
//         var p0 = Vector2.Lerp(a, b, t);
//         var p1 = Vector2.Lerp(b, c, t);
//         return Vector2.Lerp(p0, p1, t);
//     }
//     public static Vector2 cubicCurve(Vector2 a, Vector2 b, Vector2 c, Vector2 d, float t)
//     {
//         var p0 = quadraticCurve(a, b, c, t);
//         var p1 = quadraticCurve(b, c, d, t);
//         return Vector2.Lerp(p0, p1, t);
//     }

//     public static Vector2 evaluate(Anchor a, Vector2 b, float t)
//     {
//         return cubicCurve(a.o, a.p1, a.p2, b, t);
//     }
//     public static float getLength(Anchor q, Vector2 p)
//     {
//         var chord = (p - q.o).magnitude;
//         var contNet =
//         Vector2.Distance(q.o, q.p1) +
//         Vector2.Distance(q.p1, q.p2) +
//         Vector2.Distance(q.p2, p);
//         return (chord + contNet) / 2;
//     }


//     public List<Anchor> anchors;


//     public Anchor this[int index] => anchors[index];
//     public int anchorCount => anchors.Count;


//     public Curve()
//     {
//         anchors = new List<Anchor>();
//         anchors.Add(new Anchor
//         {
//             o = Vector2.left,
//             p1 = new Vector2(-.5f, .5f),
//             p2 = new Vector2(.5f, -.5f),
//         });

//         anchors.Add(new Anchor
//         {
//             o = Vector2.right,
//         }
//         );
//     }


//     public float totalLength
//     {
//         get
//         {
//             var f = 0f;
//             for (int i = 0; i < anchors.Count; i++)
//             {
//                 if (i == anchors.Count - 1)
//                     return f;
//                 var a = anchors[i];
//                 var b = anchors[i + 1];
//                 f += getLength(a, b.o);
//             }
//             return f;
//         }
//     }
// }


// [System.Serializable]
// public class Anchor
// {
//     public Vector2 o, p1, p2;
//     public Anchor() { }
//     public Anchor(Vector2 o)
//     {
//         this.o = o;
//     }
//     public Anchor(Vector2 a, Vector2 b, Vector2 c)
//     {
//         this.o = a;
//         this.p1 = b;
//         this.p2 = c;
//     }

//     public Vector2 this[int i]
//     {
//         get
//         {
//             switch (i)
//             {
//                 case 0: return o;
//                 case 1: return p1;
//                 case 2: return p2;
//             }
//             throw new System.Exception("Not exist " + i);
//         }
//         set
//         {
//             switch (i)
//             {
//                 case 0: o = value; break;
//                 case 1: p1 = value; break;
//                 case 2: p2 = value; break;
//                 default:
//                     throw new System.Exception("Not exist " + i);

//             }

//         }
//     }
// }