using UnityEngine;

public class enemy : MonoBehaviour
{
    
    void start()
    {



    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        //화살에 닿았는지 판별
        if (collision.gameObject.layer == LayerMask.NameToLayer("arrow"))
        {
            Debug.Log("x_x");
         
        }
    }
}