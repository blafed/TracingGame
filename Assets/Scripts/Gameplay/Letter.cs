using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.Sprites;
public class Letter : MonoBehaviour
{

    SpriteShapeController shape;
    Spline spline => shape.spline;
    private void Awake()
    {
        shape = GetComponent<SpriteShapeController>();
    }



    private void FixedUpdate()
    {

    }

}