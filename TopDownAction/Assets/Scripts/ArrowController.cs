using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float deleteTime = 3;   // ���� �ð�

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, deleteTime); // ���� �ð��� �����ϱ�
    }
    // ���� ������Ʈ�� ����
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ������ ���� ������Ʈ�� �ڽ����� �ϱ�
        // ���� ȭ�� ����ä �������� �ϱ⶧��
        transform.SetParent(collision.transform);

        // �浹 ������ ��Ȱ��
        GetComponent<CircleCollider2D>().enabled = false;

        // ���� �ùķ��̼��� ��Ȱ��
        GetComponent<Rigidbody2D>().simulated = false;
    }
}
