using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenManager : MonoBehaviour
{
    ObjectGenPoint[] objGens;   //���� ��ġ�Ǿ��ִ� ObjectGenPoint �迭

    // Start is called before the first frame update
    void Start()
    {
        objGens = GameObject.FindObjectsOfType<ObjectGenPoint>();
    }

    // Update is called once per frame
    void Update()
    {
        //ItemData ã��
        ItemData[] items = GameObject.FindObjectsOfType<ItemData>();
        //�ݺ������� ȭ�� ã��
        for (int i = 0; i < items.Length; i++)
        {
            ItemData item = items[i];
            if (item.type == ItemType.arrow)
            {
                return; //ȭ���� ������ �ƹ��͵� ���� �ʰ� �޼��忡�� ����������
            }
        }
        //�÷��̾ �����ϴ����� ȭ���� �� Ȯ��
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (ItemKeeper.hasArrows == 0 && player != null)
        {
            //ȭ�� ������ 0�̰� �÷��̾ �����ϸ�
            //�迭�� ���� ���� �ȿ��� ���� ����
            int index = Random.Range(0, objGens.Length);
            ObjectGenPoint objgen = objGens[index];
            objgen.ObjectCreate();   //������ ��ġ
        }
    }
}
