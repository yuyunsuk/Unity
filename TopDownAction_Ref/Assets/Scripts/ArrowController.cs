using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float deleteTime = 3;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, deleteTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ������ ���� �浹�ϴ� ���, ���Ϳ� ���� ���·� �������� �ϱ� ������
        // �浹��ü�� �ڽ����� ������
        transform.SetParent(collision.transform);
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
    }
}
