using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int arrangeId = 0;       // �ĺ��� ����ϱ� ���� ��

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // ���踦 ������������
            if (ItemKeeper.hasKeys > 0)
            {
                ItemKeeper.hasKeys--;       // ���踦 �ϳ� ����
                Destroy(this.gameObject);   // �� ����
            }
        }
    }
}
