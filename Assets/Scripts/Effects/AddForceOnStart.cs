using System.Collections;
using UnityEngine;

public class AddForceOnStart : MonoBehaviour
{
    public float delay = 0;
    public Vector2 force = new Vector2(0, 10);
    public bool changeGraivtyScale = false;
    public float gravityScale = 1;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(delay);
        var rb = GetComponent<Rigidbody2D>();
        rb.AddForce(force, ForceMode2D.Impulse);
        if (changeGraivtyScale)
            rb.gravityScale = gravityScale;
    }
}