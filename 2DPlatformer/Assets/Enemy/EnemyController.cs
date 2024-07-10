using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3.0f;          // 이동 속도
    public string direction = "left";   // 방향 right or left 
    public float range = 0.0f;          // 움직이는 범위
    Vector3 defPos;                     // 시작 위치

    // Start is called before the first frame update
    void Start()
    {
        if (direction == "right")
        {
            transform.localScale = new Vector2(-1, 1);// 방향 변경, 기존 left
        }

        // 시작 위치
        defPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (range > 0.0f) // 움직이는 볌위가 0 보다 클때
        {
            // 현재 x 위치가 시작-범위50% 보다 작으면 오른쪽으로 이동
            if (transform.position.x < defPos.x - (range / 2))
            {
                direction = "right";
                transform.localScale = new Vector2(-1, 1);// 방향 변경, 이미지 스케일로 변환, -1, 반대방향

                // 현재 x 위치가 시작+범위50% 보다 크면 왼쪽으로 이동
                if (transform.position.x > defPos.x + (range / 2)) {
                    direction = "left";
                    transform.localScale = new Vector2(1, 1);// 방향 변경, 이미지 스케일로 변환
                }
            }
        }
    }

    void FixedUpdate()
    {
        // 속도 갱신
        // Rigidbody2D 가져오기
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        if (direction == "right")
        {
            rbody.velocity = new Vector2(speed, rbody.velocity.y); // 속도 양수, 음수로 방향 전환
        }
        else
        {
            rbody.velocity = new Vector2(-speed, rbody.velocity.y); // 속도 양수, 음수로 방향 전환
        }
    }

    // 접촉
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (direction == "right")
        {
            direction = "left";
            transform.localScale = new Vector2(1, 1); // 방향 변경, 이미지 스케일로 변환
        }
        else
        {
            direction = "right";
            transform.localScale = new Vector2(-1, 1); // 방향 변경, 이미지 스케일로 변환
        }
    }
}
