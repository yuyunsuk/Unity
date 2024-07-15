using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    // ü��
    public int hp = 10;
    // ���� �Ÿ�
    public float reactionDistance = 7.0f;

    public GameObject bulletPrefab;     // �Ѿ�
    public float shootSpeed = 5.0f;     // �Ѿ� �ӵ�

    // ���������� ����
    bool inAttack = false;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (hp > 0)
        {
            // Player ���� ������Ʈ ��������
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                // �÷��̾���� �Ÿ� Ȯ��
                Vector3 plpos = player.transform.position;
                float dist = Vector2.Distance(transform.position, plpos);

                if (dist <= reactionDistance && inAttack == false)
                {
                    // ���� �� & ���� ���� �ƴϸ� ���� �ִϸ��̼�
                    inAttack = true;

                    // �ִϸ��̼� ����
                    GetComponent<Animator>().Play("BossAttack");
                    // Invoke("Attack", 0.25f); // 0.25�� �ڿ� �߻�
                }
                else if (dist > reactionDistance && inAttack)
                {
                    inAttack = false;
                    // �ִϸ��̼� ����
                    GetComponent<Animator>().Play("BossIdle");
                }
            }
            else
            {
                inAttack = false;
                // �ִϸ��̼� ����
                GetComponent<Animator>().Play("BossIdle");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Arrow")
        {
            // ������
            hp--;
            if (hp <= 0)
            {
                // �����
                // �浹 ���� ��Ȱ��
                GetComponent<CircleCollider2D>().enabled = false;
                // �ִϸ��̼� ����
                GetComponent<Animator>().Play("BossDead");
                // 1�� �ڿ� ����
                Destroy(gameObject, 1);
            }
        }
    }
    // ����
    void Attack()
    {
        // �߻� ��ġ ������Ʈ ��������
        Transform tr = transform.Find("gate");
        GameObject gate = tr.gameObject;

        // �Ѿ��� �߻��� ���� �����
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float dx = player.transform.position.x - gate.transform.position.x;
            float dy = player.transform.position.y - gate.transform.position.y;
            // ��ũ ź��Ʈ2 �Լ��� ����(����) ���ϱ�
            float rad = Mathf.Atan2(dy, dx);
            // ������ ������ ��ȯ
            float angle = rad * Mathf.Rad2Deg;
            // Prefab���� �Ѿ� ������Ʈ �����(���� �������� ȸ��)
            Quaternion r = Quaternion.Euler(0, 0, angle);
            GameObject bullet = Instantiate(bulletPrefab, gate.transform.position, r);
            float x = Mathf.Cos(rad);
            float y = Mathf.Sin(rad);
            Vector3 v = new Vector3(x, y) * shootSpeed;
        
            // �߻�
            Rigidbody2D rbody = bullet.GetComponent<Rigidbody2D>();
            rbody.AddForce(v, ForceMode2D.Impulse);
        }
    }
}
