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
            if (otherTarget != null)
            {
                Vector2 pos = Vector2.Lerp(player.transform.position, otherTarget.transform.position, 0.5f); // Lerp 선형보간, 중간값 계산, 부드러운 움직임에 많이 쓰임

                // 플레이어의 위치와 연동
                transform.position = new Vector3(pos.x, pos.y, -10);
            }
            else
            {
                // 플레이어의 위치와 연동
                transform.position = new Vector3(
                    player.transform.position.x,
                    player.transform.position.y,
                    -10);
            }
        }
    }
}
