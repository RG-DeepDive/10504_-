using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{

    public float maxSpeed;
    Rigidbody2D rigid;

    public float movePower;
    public float jumpPower;
    public float jumpHoldPower;
    public float maxJumpTime;

    private float jumpTime;
    private bool isJumping;
    private bool isGrounded;

    public float jumpBufferTime = 0.15f;

    private float jumpBufferCounter;

    



     void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();   // 물리
    }

    
    void Start()
    {
        
    }

    
    void Update()
    {
        //점프 시스템
        // 점프 시작
        if (jumpBufferCounter > 0 && isGrounded)
        {
            rigid.linearVelocity = new Vector2(rigid.linearVelocity.x, 0);

            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

            isJumping = true;
            isGrounded = false;
            jumpTime = 0;
            
            // 점프 예약 사용 완료
            jumpBufferCounter = 0;
        }

        // 점프키 활공
        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpTime < maxJumpTime)
            {
                rigid.AddForce(Vector2.up * jumpHoldPower, ForceMode2D.Force);

                jumpTime += Time.deltaTime;
            }
        }

        // 점프키 떼면 점프 멈춤
        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;

            // 짧게 누르면 빨리 떨어짐
            if (rigid.linearVelocity.y > 0)
            {
                rigid.linearVelocity = new Vector2(
                    rigid.linearVelocity.x,
                    rigid.linearVelocity.y * 0.5f
                );
            }
        }
        // 중력 다루는거
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            // 아래키 눌렀을 때 중력
            rigid.gravityScale = 1.2f;
        }
        else if (Input.GetButton("Jump") && rigid.linearVelocity.y < 0)
        {
            // 점프키 눌렀을 때 중력
            rigid.gravityScale = 0.2f;
        }
        else
        {
            // 평소 중력
            rigid.gravityScale = 1f;
        }
        
         if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
        // 가속기
         if (Input.GetKey(KeyCode.U))
         { 
            maxSpeed = 13f;
            rigid.mass = 1.75f;
            jumpPower = 6f;
            movePower = 0.12f;
            
         }
         // 평소
         else
         {
            maxSpeed = 6.5f;
            rigid.mass = 1f;
            jumpPower = 4f;
            movePower = 0.15f;
         }
    }

    void FixedUpdate()
{
    // 좌우 움직임
    float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h * movePower, ForceMode2D.Impulse);
        if(rigid.linearVelocity.x > maxSpeed) // 오른쪽 최고속도
        {
            rigid.linearVelocity = new Vector2(maxSpeed, rigid.linearVelocity.y);
        }
        else if (rigid.linearVelocity.x < maxSpeed*(-1))// 왼쪽 최고속도
        {
            rigid.linearVelocity = new Vector2(maxSpeed*(-1), rigid.linearVelocity.y);
        }
}

// 바닥에 닿았는지 확인
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isJumping = false;
        }
    }
}

