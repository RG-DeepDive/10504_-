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

    public float jumpTime;
    public bool isJumping;
    public bool isGrounded;
    public bool isMoving;
    public bool isAiming;

    public float jumpBufferTime = 0.15f;

    private bool jumpHolding;
    private float jumpBufferCounter;

    public bow bow;

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

    void FixedUpdate()
    {
        // 활을 당기는 중에는 이동 불가
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

        // 가속기
        if (Input.GetKey(KeyCode.U))
        {
            maxSpeed = 13f;
            rigid.mass = 1.75f;
            jumpPower = 9f;
            movePower = 0.12f;
        }
        else
        {
            maxSpeed = 6.5f;
            rigid.mass = 1f;
            jumpPower = 6f;
            movePower = 0.15f;
        }
    }
}