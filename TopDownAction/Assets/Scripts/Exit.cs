using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���Ա� ��ġ
public enum ExitDirection
{
    right,  // ������
    left,   // ����
    down,   // �Ʒ���
    up,     // ����
}

public class Exit : MonoBehaviour
{
    public string sceneName = "";                        // �̵��� �� �̸�
    public int doorNumber = 0;                           // �� ��ȣ
    public ExitDirection direction = ExitDirection.down; // ���� ��ġ

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player Attech!!!");
            RoomManager.ChangeScene(sceneName, doorNumber);
        }
    }
}

