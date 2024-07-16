using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity; // 디폴트

    void Start ()
    {
        m_Animator = GetComponent<Animator> ();
        m_Rigidbody = GetComponent<Rigidbody> ();
        m_AudioSource = GetComponent<AudioSource> ();
    }

    void FixedUpdate ()
    {
        float horizontal = Input.GetAxis ("Horizontal");
        float vertical = Input.GetAxis ("Vertical");
        
        m_Movement.Set(horizontal, 0f, vertical); // y 는 0, 전후(vertical), 좌우(horizontal)
        m_Movement.Normalize ();

        bool hasHorizontalInput = !Mathf.Approximately (horizontal, 0f); // 좌우가 거의 0 이 아니면, 확실하게 방향키 입력이 들어오면
        bool hasVerticalInput   = !Mathf.Approximately (vertical, 0f);   // 전후가 거의 0 이 아니면, 확실하게 방향키 입력이 들어오면
        bool isWalking          = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool ("IsWalking", isWalking);
        
        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play(); // 걷는 소리 재생
            }
        }
        else
        {
            m_AudioSource.Stop (); // 멈추는 소리 재생
        }

        Vector3 desiredForward = Vector3.RotateTowards (transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f); // 어느방향으로 돌려야 하는지 계산, Time.deltaTime 를 곱한다는건 프레임당
        m_Rotation = Quaternion.LookRotation (desiredForward); // LookRotation 함수는 해당 방향으로 돌려줌
    }

    void OnAnimatorMove () // 애니메이션의 루트 모션 움직임 처리 콜백 함수, 프레임마다 (Rigidbody, 콜라이더 따라감)
    {
        m_Rigidbody.MovePosition (m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude); // MovePosition 이동시키는 코드 (m_Movement: 방향), (m_Animator.deltaPosition.magnitude: 이동 거리)
        m_Rigidbody.MoveRotation (m_Rotation);
    }
}