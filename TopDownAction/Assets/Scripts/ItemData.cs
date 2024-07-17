using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ����
public enum ItemType
{
    arrow,  // ȭ��
    key,    // ����
    life,   // ����
}

public class ItemData : MonoBehaviour
{
    SaveLoadManager saveLoadManager;

    public ItemType type;       // �������� ����
    public int count = 1;       // ������ ��

    public int arrangeId = 0;   // �ĺ��� ���� ��

    // Start is called before the first frame update
    void Start()
    {
        saveLoadManager = GameObject.FindObjectOfType<SaveLoadManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    // ���� (����)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Player �� �浹�� �Ͼ����
        if (collision.gameObject.tag == "Player")
        {
            // ����
            if (type == ItemType.key)
            {
                // ���� +1
                ItemKeeper.hasKeys += 1;
            }
            else if (type == ItemType.arrow)
            {
                // ȭ�� +1
                ArrowShoot shoot = collision.gameObject.GetComponent<ArrowShoot>();
                ItemKeeper.hasArrows += count;
            }
            else if (type == ItemType.life)
            {
                // ����
                if (PlayerController.hp < 3)
                {
                    // HP �� 3���ϸ� �߰�
                    PlayerController.hp++;

                    // HP ����
                    PlayerPrefs.SetInt("PlayerHP", PlayerController.hp);
                }
            }
            // ��ġ Id ����
            saveLoadManager.SetSceneData(this.gameObject.name, false);

            Debug.Log("ItemKeeper.hasKeys: "   + ItemKeeper.hasKeys);
            Debug.Log("ItemKeeper.hasArrows: " + ItemKeeper.hasArrows);
            Debug.Log("ItemKeeper.life: "      + PlayerController.hp);

            //++++ ������ ȹ�� ���� ++++
            // �浹 ���� ��Ȱ��
            gameObject.GetComponent<CircleCollider2D>().enabled = false;

            // �������� Rigidbody2D��������
            Rigidbody2D itemBody = GetComponent<Rigidbody2D>();

            // �߷� ����
            itemBody.gravityScale = 2.5f; // 1.0f �� �߷� �״��, 2.5f �� �߷��� 2.5��
            //itemBody.gravityScale = 1.0f; // 1.0f �� �߷� �״��, 2.5f �� �߷��� 2.5�� (�ȶپ����)

            // ���� Ƣ������� ����
            itemBody.AddForce(new Vector2(0, 6), ForceMode2D.Impulse); // �߷��� 1.0f �϶� �ʹ� ���� �ö�

            // 0.5�� �ڿ� ����
            Destroy(gameObject, 0.5f);
        }
    }
}

