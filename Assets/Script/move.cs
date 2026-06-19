using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    //최대 속도
    public float maxSpeed;
    Rigidbody2D rigid;
    SpriteRenderer rend;

    //기본 속도
    public float movePower;

    //기본 점프 높이
    public float jumpPower;
    //점프키를 누르고있을때 상승하는 높이
    public float jumpHoldPower;
    //최대 점프 유지 시간
    public float maxJumpTime;
    //점프 중인 시간
    public float jumpTime;

    //점프를 하고 있는지
    public bool isJumping;
    //땅에 닿았는지
    public bool isGrounded;
    //움직이고 있는지
    public bool isMoving;
    //활을 조준하고 있는지
    public bool isAiming;

    //점프 판정 허용시간
    public float jumpBufferTime = 0.15f;
    //점프키를 누르고 있는지
    private bool jumpHolding;
    //점프 저장 타이머
    private float jumpBufferCounter;

    //활
    public bow bow;

    //돈
    public float money = 0f;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        // 활 스크립트 찾기
        bow = GetComponent<bow>();
    }

    void Update()
    {
        // 점프키 상태 저장
        jumpHolding = Input.GetButton("Jump");

        // 점프 예약
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // 점프 시작
        if (jumpBufferCounter > 0 && isGrounded)
        {
            rigid.linearVelocity =
                new Vector2(
                    rigid.linearVelocity.x,
                    0
                );

            rigid.AddForce(
                Vector2.up * jumpPower,
                ForceMode2D.Impulse
            );

            isJumping = true;
            isGrounded = false;
            jumpTime = 0;

            jumpBufferCounter = 0;
        }

        // 점프 길게 누르기
        if (jumpHolding && isJumping)
        {
            if (jumpTime < maxJumpTime)
            {
                rigid.AddForce(
                    Vector2.up * jumpHoldPower,
                    ForceMode2D.Force
                );

                jumpTime += Time.deltaTime;
            }
        }

        // 점프키를 떼면
        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;

            // 짧게 누르면 빨리 떨어짐
            if (rigid.linearVelocity.y > 0)
            {
                rigid.linearVelocity =
                    new Vector2(
                        rigid.linearVelocity.x,
                        rigid.linearVelocity.y * 0.5f
                    );
            }
        }

        // 중력 조절

        // 급강하
        if (
            Input.GetKey(KeyCode.S)
            || Input.GetKey(KeyCode.DownArrow)
        )
        {
            rigid.gravityScale = 1.2f;
        }

        // 활공
        else if (
            jumpHolding
            && !isGrounded
            && rigid.linearVelocity.y < 0
        )
        {
            rigid.gravityScale = 0.2f;
        }

        // 기본
        else
        {
            rigid.gravityScale = 1f;
        }







    }

     // 땅에 닿았을 때
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("gold"))
        {
            money += 20f;
        }
        else if (other.CompareTag("silver"))
        {
            money += 10f;
        }
        else if (other.CompareTag("bronze"))
        {
            money += 5f;
        }
    }

    void FixedUpdate()
    {
        //이동, 활을 당기는 중에는 이동 불가
        if (!bow.isCharging)
        {
            float h =
                Input.GetAxisRaw("Horizontal");

            rigid.AddForce(
                Vector2.right
                * h
                * movePower,
                ForceMode2D.Impulse
            );

            if (
                rigid.linearVelocity.x
                > maxSpeed
            )
            {
                
                rigid.linearVelocity =
                    new Vector2(
                        maxSpeed,
                        rigid.linearVelocity.y
                    );
            }
            else if (
                rigid.linearVelocity.x
                < -maxSpeed
            )
            {
                
                rigid.linearVelocity =
                    new Vector2(
                        -maxSpeed,
                        rigid.linearVelocity.y
                    );
                   
                   
            }
        }

        // 질량 증가
        if (Input.GetKey(KeyCode.U))
        {
            maxSpeed = 13f;
            rigid.mass = 1.75f;
            jumpPower = 7.5f;
            movePower = 0.12f;
        }
        else
        {
            maxSpeed = 6.5f;
            rigid.mass = 1f;
            jumpPower = 5f;
            movePower = 0.15f;
        }
    }
}