using UnityEngine;

public class InputManager : MonoBehaviour
{
    public bool isEnter;
    public Vector2 point;


    private void Update()
    {
        isEnter = Input.GetMouseButton(0);
        point = Input.mousePosition;
    }
}