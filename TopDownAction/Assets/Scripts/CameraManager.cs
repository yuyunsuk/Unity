using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject otherTarget;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // �÷��̾��� ��ġ�� ����
            transform.position = new Vector3(player.transform.position.x,
                             player.transform.position.y,
                                             -10);
        }
    }
}
