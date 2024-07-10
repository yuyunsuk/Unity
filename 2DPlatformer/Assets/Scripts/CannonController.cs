using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public GameObject objPrefab;        // 발생시키는 Prefab 데이터 
    public float delayTime = 3.0f;      // 지연시간
    public float fireSpeedX = -4.0f;    // 발사 벡터  X
    public float fireSpeedY = 0.0f;     // 발사 벡터 Y
    public float length = 8.0f;

    GameObject player;                  // 플레이어
    GameObject gateObj;                 // 발사구
    float passedTimes = 0;              // 경과 시간 

    // Start is called before the first frame update
    void Start()
    {
        // 발사구 오브젝트 얻기
        // Transform tr = transform.Find("gate");
        // gateObj = tr.gameObject;
        gateObj = transform.Find("gate").gameObject;

        // 플레이어
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // 발사 시간 판정
        passedTimes += Time.deltaTime;
        // 거리 확인
        if (CheckLength(player.transform.position))
        {
            if (passedTimes > delayTime)
            {
                // 발사!!
                passedTimes = 0;
                // 발사 위치
                Vector3 pos = new Vector3(gateObj.transform.position.x,
                                          gateObj.transform.position.y,
                                          transform.position.z);

                // Prefab 으로 GameObject 만들기
                // 첫번째 사용할 프리펩, 두번째 배치위치, 세번째 회전값
                // Quaternion => 오일러각을 사용하지 않고 Quaternion 를 사용, Quaternion.identity => 회전하지 않음
                GameObject obj = Instantiate(objPrefab, pos, Quaternion.identity);

                // 발사 방향
                Rigidbody2D rbody = obj.GetComponent<Rigidbody2D>();
                Vector2 v = new Vector2(fireSpeedX, fireSpeedY); // -4(우에서 좌로), 0(상하는 변화 없음)

                // 발사를 하기 위해서는 Velocity(속도) 를 주거나 AddForce(힘을 가하기) 를 주어야 함
                rbody.AddForce(v, ForceMode2D.Impulse);
            }
        }
    }

    // 거리확인
    bool CheckLength(Vector2 targetPos)
    {
        bool ret = false;
        float d = Vector2.Distance(transform.position, targetPos); // 거리 구하기
        if (length >= d)
        {
            ret = true;
        }
        return ret;
    }
}
