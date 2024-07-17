using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    SaveLoadManager saveLoadManager;

    public Sprite openImage;        // ���� �̹���
    public GameObject itemPrefab;   // ����ִ� �������� ������

    public bool isClosed = true;    // true=�����ִ١�false=���� �ִ�.

    public int arrangeId = 0;       // ��ġ �ĺ��� ���

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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isClosed && collision.gameObject.tag == "Player")
        {
            // ���ڰ� ���� �ִ� ���¿��� �÷��̾�� ����
            GetComponent<SpriteRenderer>().sprite = openImage;
            isClosed = false;   // ���� ���·� �ϱ�
            if (itemPrefab != null)
            {
                // ���������� ������ �����
                Instantiate(itemPrefab, transform.position, Quaternion.identity);
            }

            // ��ġ Id ���
            // SaveDataManager.SetArrangeId(arrangeId, gameObject.tag);
            saveLoadManager.SetSceneData(this.gameObject.name, false);
        }
    }
}
