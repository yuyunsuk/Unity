using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float deleteTime = 3.0f; // ������ �ð� ����

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, deleteTime); // ���� ����
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject); // ������ �߻��ϸ� ��� ����
    }
}
