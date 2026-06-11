using UnityEngine;

public class enemy : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("arrow"))
        {
            Debug.Log("x_x");
        }
    }
}