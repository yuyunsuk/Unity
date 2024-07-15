using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShoot : MonoBehaviour
{
    public float shootSpeed = 12.0f;   // ȭ�� �ӵ�
    public float shootDelay = 0.25f;   // �߻� ���� (������ Input(��Ÿ)�� ����)
    public GameObject bowPrefab;       // Ȱ�� ������
    public GameObject arrowPrefab;     // ȭ���� ������

    bool inAttack = false;             // ���� �� ����
    GameObject bowObj;                 // Ȱ�� ���� ������Ʈ

    // Start is called before the first frame update
    void Start()
    {
        ItemKeeper.hasArrows = 0; // Test ItemKeeper.hasArrows = 1000

        // Ȱ�� �÷��̾� ĳ���Ϳ� ��ġ
        Vector3 pos = transform.position;
        bowObj = Instantiate(bowPrefab, pos, Quaternion.identity); // ȸ���� �״�� ����
        bowObj.transform.SetParent(transform); // Ȱ�� �θ�� �÷��̾� ĳ���͸� ����
    }

    // Update is called once per frame
    void Update()
    {

        // if ((Input.GetButtonDown("Fire3"))) // ���� ����ƮŰ
        if ((Input.GetButtonDown("Jump"))) // �����̽�Ű
        {
            // ���� Ű�� ����
            Attack();
        }
        // Ȱ�� ȸ���� �켱����
        float bowZ = -1;    // Ȱ�� Z�� (ĳ���ͺ��� ������ ����) => Order in Layer ������ �� ���� ���

        PlayerController plmv = GetComponent<PlayerController>();

        // if (plmv.angleZ > 30 && plmv.angleZ < 150)
        if (plmv.angleZ >= 45 && plmv.angleZ < 135)
        {
            // �� ����
            bowZ = 1;       // Ȱ�� Z�� (ĳ���� ���� �ڷ� ����)
        }
        //Ȱ�� ȸ��
        bowObj.transform.rotation = Quaternion.Euler(0, 0, plmv.angleZ); // z ���⸸ ȸ��
        
        // Ȱ�� �켱����
        bowObj.transform.position = new Vector3(transform.position.x,
                                    transform.position.y, bowZ); // Ȱ�� ��ġ ������Ʈ
    }
    //����
    public void Attack()
    {
        // ȭ���� ������ ���� & ���� ���� �ƴ�
        if (ItemKeeper.hasArrows > 0 && inAttack == false) // ȭ���� ������ �ְ�, ���� ���̸� ������ ���ϵ��� ó��
        {
            ItemKeeper.hasArrows -= 1;	// ȭ���� �Ҹ�
            inAttack = true;		    // ���� ������ ����

            // ȭ�� �߻�
            PlayerController playerCnt = GetComponent<PlayerController>();
            float angleZ = playerCnt.angleZ; //ȸ�� ����

            // ȭ���� ���� ������Ʈ �����(���� �������� ȸ��)
            Quaternion r = Quaternion.Euler(0, 0, angleZ); // 
            GameObject arrowObj = Instantiate(arrowPrefab, transform.position, r); // ȭ���� ���� ȸ��(Prefab, ��ġ, ȸ��)

            // ȭ���� �߻��� ���� ���� ���� ����(���� 1 ��)
            float x = Mathf.Cos(angleZ * Mathf.Deg2Rad); // (x / ����) => x
            float y = Mathf.Sin(angleZ * Mathf.Deg2Rad); // (y / ����) => y
            Vector3 v = new Vector3(x, y) * shootSpeed;

            // ȭ�쿡 ���� ���ϱ�
            Rigidbody2D body = arrowObj.GetComponent<Rigidbody2D>();
            body.AddForce(v, ForceMode2D.Impulse);

            // ���� ���� �ƴ����� ����
            Invoke("StopAttack", shootDelay); // �񵿱� �Լ� ȣ��, 0.25�� �ڿ� StopAttack ȣ��
        }
    }
    // ���� ����
    public void StopAttack()
    {
        inAttack = false; // ���� ���� �ƴ����� ����
    }
}
