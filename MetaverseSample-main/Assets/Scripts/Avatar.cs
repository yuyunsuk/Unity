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
        // ������Ʈ�� ������ �����Ѵ�.
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();

        // Save�� �ƹ�Ÿ ������ �ε��Ѵ�.
        GetComponent<CharacterCustomizationManager>().Load(); 
    }

    void FixedUpdate()
    {
        // �̵� �Է��� �޴´�.
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // ���� �Է��� ���Ϳ� �����Ѵ�.
        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();        

        // �̵� �������� ȸ�������� ���Ѵ�
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);

        // ���� �̵� ���Ϳ� ȸ���� �����Ѵ�.
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * Time.deltaTime);
        m_Rigidbody.MoveRotation(m_Rotation);

        // �̵����� ���� �ִϸ������� �Ķ���͸� �����Ѵ�.         
        bool isWalking = m_Movement.sqrMagnitude > 0f;
        m_Animator.SetBool("IsWalking", isWalking);
    }
}
