using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bow : MonoBehaviour
{
    // 화살 프리팹
    public GameObject arrowPrefab;

    // 화살 생성 위치
    public Transform firePoint;

    // 활 회전 속도
    public float rotateSpeed = 120f;

    // 최소, 최대 위력
    public float minPower = 5f;
    public float maxPower = 20f;

    // 최대 차징 시간
    public float maxChargeTime = 2f;

    // 활 쿨타임
    public float coolTime = 1f;

    // 활시위를 당기고 있는지
    public bool isCharging;

    // 발사 가능 여부
    private bool canShoot = true;

    // 차징 시간
    private float chargeTime;

    // 장전된 화살
    private GameObject currentArrow;

    void Update()
    {
        // 활시위 당기기 시작
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (canShoot && !isCharging)
            {
                StartCharge();
            }
        }

        // 활 조준
        if (isCharging)
        {
            AimBow();
        }

        // 활시위 놓기
        if (Input.GetKeyUp(KeyCode.I))
        {
            if (isCharging)
            {
                ShootArrow();
            }
        }
    }

    // 활시위 당기기
    void StartCharge()
    {
        isCharging = true;

        chargeTime = 0;

        // 장전 화살 생성
        currentArrow =
            Instantiate(
                arrowPrefab,
                firePoint.position,
                firePoint.rotation
            );

        // 플레이어를 따라다니게
        currentArrow.transform.SetParent(firePoint);

        // 장전 상태에서는 물리 끄기
        Rigidbody2D rb =
            currentArrow.GetComponent<Rigidbody2D>();

        rb.simulated = false;
    }

    // 활 조준
    void AimBow()
    {
        // 차징
        chargeTime += Time.deltaTime;

        if (chargeTime > maxChargeTime)
        {
            chargeTime = maxChargeTime;
        }

        // A키 : 반시계 회전
        if (Input.GetKey(KeyCode.A))
        {
            firePoint.Rotate(
                0,
                0,
                rotateSpeed * Time.deltaTime
            );
        }

        // D키 : 시계 회전
        if (Input.GetKey(KeyCode.D))
        {
            firePoint.Rotate(
                0,
                0,
                -rotateSpeed * Time.deltaTime
            );
        }
    }

    // 화살 발사
    void ShootArrow()
    {
        isCharging = false;

        // 플레이어와 분리
        currentArrow.transform.parent = null;

        Rigidbody2D rb =
            currentArrow.GetComponent<Rigidbody2D>();

        // 물리 활성화
        rb.simulated = true;

        // 차징량 계산
        float power =
            Mathf.Lerp(
                minPower,
                maxPower,
                chargeTime / maxChargeTime
            );

        // 화살 발사
        rb.linearVelocity =
                firePoint.up * power;

        currentArrow = null;

        // 쿨타임 시작
        StartCoroutine(BowCoolDown());
    }

    // 활 쿨타임
    IEnumerator BowCoolDown()
    {
        canShoot = false;

        yield return new WaitForSeconds(coolTime);

        canShoot = true;
    }
}