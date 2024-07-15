using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualPad : MonoBehaviour
{
    public float MaxLength = 70;    // ���� �����̴� �ִ� �Ÿ�
    public bool is4DPad = false;    // �����¿�� �����̴��� ����
    GameObject player;              // ���� �� �÷��̾� GameObject
    Vector2 defPos;                 // ���� �ʱ� ��ǥ
    Vector2 downPos;                // ��ġ ��ġ

    // Start is called before the first frame update
    void Start()
    {
        // �÷��̾� ĳ���� ��������
        player = GameObject.FindGameObjectWithTag("Player");
        // ���� �ʱ� ��ǥ
        defPos = GetComponent<RectTransform>().localPosition;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // �ٿ� �̺�Ʈ
    public void PadDown()
    {
        // ���콺 ����Ʈ�� ��ũ�� ��ǥ
        downPos = Input.mousePosition;
    }
    // �巡�� �̺�Ʈ 
    public void PadDrag()
    {
        // ���콺 ����Ʈ�� ��ũ�� ��ǥ
        Vector2 mousePosition = Input.mousePosition;
        // ���ο� �� ��ġ ���ϱ�
        Vector2 newTabPos = mousePosition - downPos;// ���콺 �ٿ� ��ġ�� ������ �̵� �Ÿ�
        if (is4DPad == false)
        {
            newTabPos.y = 0; // Ⱦ��ũ�� �� ����  Y ���� 0 ���� �Ѵ�.
        }
        // �̵� ���� ����ϱ�
        Vector2 axis = newTabPos.normalized; // ���͸� ����ȭ
        // �� ���� �Ÿ� ���ϱ�
        float len = Vector2.Distance(defPos, newTabPos);
        if (len > MaxLength)
        {
            // �Ѱ�Ÿ��� �Ѱ�� ������ �Ѱ� ��ǥ�� ����
            newTabPos.x = axis.x * MaxLength;
            newTabPos.y = axis.y * MaxLength;
        }
        // �� �̵� ��Ű��
        GetComponent<RectTransform>().localPosition = newTabPos;
        // �÷��̾� ĳ���� �̵� ��Ű��
        PlayerController plcnt = player.GetComponent<PlayerController>();
        plcnt.SetAxis(axis.x, axis.y);
    }
    // �� �̺�Ʈ
    public void PadUp()
    {
        // �� ��ġ�� �ֱ�ȭ 
        GetComponent<RectTransform>().localPosition = defPos;
        // �÷��̾� ĳ���� ���� ��Ű��
        PlayerController plcnt = player.GetComponent<PlayerController>();
        plcnt.SetAxis(0, 0);
    }

    // ����
    public void Attack() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        ArrowShoot shoot = player.GetComponent<ArrowShoot>();
        shoot.Attack();
    }

}
