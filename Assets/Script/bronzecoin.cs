using UnityEngine;

public class bronzecoin : MonoBehaviour

{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

   


    
    // 땅에 닿았을 때
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);

            Debug.Log("냠");
        }
    }
}