using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : MonoBehaviour
{
    Rigidbody2D rigid;

    public float rotateSpeed = 10f;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (rigid.linearVelocity.sqrMagnitude > 0.1f)
        {
            float angle =
                Mathf.Atan2(
                    rigid.linearVelocity.y,
                    rigid.linearVelocity.x
                ) * Mathf.Rad2Deg - 90f;

            transform.rotation =
                Quaternion.Lerp(
                    transform.rotation,
                    Quaternion.Euler(0, 0, angle),
                    rotateSpeed * Time.deltaTime
                );
        }
    }

    void Start()
    {
        //일정시간이 지나면 맵에서 화살 지우기
        Destroy(gameObject, 15f);
    }

}