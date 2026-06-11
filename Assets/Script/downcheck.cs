using UnityEngine;

public class downcheck : MonoBehaviour
{
    public move pmove;

    // 땅에 닿았을 때
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            pmove.isGrounded = true;
            pmove.isJumping = false;
            pmove.jumpTime = 0;

            Debug.Log("착지");
        }
    }

    // 땅에서 떨어졌을 때
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            pmove.isGrounded = false;

            Debug.Log("공중");
        }
    }
}