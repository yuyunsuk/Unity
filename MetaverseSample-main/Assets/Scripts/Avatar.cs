using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour
{
    public float turnSpeed = 20f;

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    void Start()
    {
        // 컴포넌트를 변수에 저장한다.
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();

        // Save된 아바타 정보를 로드한다.
        GetComponent<CharacterCustomizationManager>().Load(); 
    }

    void FixedUpdate()
    {
        // 이동 입력을 받는다.
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // 받은 입력을 벡터에 저장한다.
        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();        

        // 이동 방향으로 회전방향을 구한다
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);

        // 구한 이동 벡터와 회전을 적용한다.
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * Time.deltaTime);
        m_Rigidbody.MoveRotation(m_Rotation);

        // 이동값에 따라 애니메이터의 파라미터를 변경한다.         
        bool isWalking = m_Movement.sqrMagnitude > 0f;
        m_Animator.SetBool("IsWalking", isWalking);
    }
}
