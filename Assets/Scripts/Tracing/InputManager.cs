using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager o;

    public bool isEnter;
    public Vector2 point;

    private void Awake()
    {
        o = this;
    }


    private void Update()
    {
        isEnter = Input.GetMouseButton(0);
        point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}