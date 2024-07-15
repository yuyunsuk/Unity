using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShoot : MonoBehaviour
{
    public float shootSpeed = 12.0f;   // 화살 속도
    public float shootDelay = 0.25f;   // 발사 간격 (유저의 Input(연타)를 막음)
    public GameObject bowPrefab;       // 활의 프리펩
    public GameObject arrowPrefab;     // 화살의 프리펩

    bool inAttack = false;             // 공격 중 여부
    GameObject bowObj;                 // 활의 게임 오브젝트

    // Start is called before the first frame update
    void Start()
    {
        ItemKeeper.hasArrows = 0; // Test ItemKeeper.hasArrows = 1000

        // 활을 플레이어 캐릭터에 배치
        Vector3 pos = transform.position;
        bowObj = Instantiate(bowPrefab, pos, Quaternion.identity); // 회전은 그대로 적용
        bowObj.transform.SetParent(transform); // 활의 부모로 플레이어 캐릭터를 설정
    }

    // Update is called once per frame
    void Update()
    {

        // if ((Input.GetButtonDown("Fire3"))) // 왼쪽 쉬프트키
        if ((Input.GetButtonDown("Jump"))) // 스페이스키
        {
            // 공격 키가 눌림
            Attack();
        }
        // 활의 회전과 우선순위
        float bowZ = -1;    // 활의 Z값 (캐릭터보다 앞으로 설정) => Order in Layer 조정이 더 좋은 방법

        PlayerController plmv = GetComponent<PlayerController>();

        // if (plmv.angleZ > 30 && plmv.angleZ < 150)
        if (plmv.angleZ >= 45 && plmv.angleZ < 135)
        {
            // 위 방향
            bowZ = 1;       // 활의 Z값 (캐릭터 보다 뒤로 설정)
        }
        //활의 회전
        bowObj.transform.rotation = Quaternion.Euler(0, 0, plmv.angleZ); // z 방향만 회전
        
        // 활의 우선순위
        bowObj.transform.position = new Vector3(transform.position.x,
                                    transform.position.y, bowZ); // 활의 위치 업데이트
    }
    //공격
    public void Attack()
    {
        // 화살을 가지고 있음 & 공격 중이 아님
        if (ItemKeeper.hasArrows > 0 && inAttack == false) // 화살을 가지고 있고, 공격 중이면 공격을 못하도록 처리
        {
            ItemKeeper.hasArrows -= 1;	// 화살을 소모
            inAttack = true;		    // 공격 중으로 설정

            // 화살 발사
            PlayerController playerCnt = GetComponent<PlayerController>();
            float angleZ = playerCnt.angleZ; //회전 각도

            // 화살의 게임 오브젝트 만들기(진행 방향으로 회전)
            Quaternion r = Quaternion.Euler(0, 0, angleZ); // 
            GameObject arrowObj = Instantiate(arrowPrefab, transform.position, r); // 화살을 만들어서 회전(Prefab, 위치, 회전)

            // 화살을 발사할 방향 단위 벡터 생성(빗변 1 임)
            float x = Mathf.Cos(angleZ * Mathf.Deg2Rad); // (x / 빗변) => x
            float y = Mathf.Sin(angleZ * Mathf.Deg2Rad); // (y / 빗변) => y
            Vector3 v = new Vector3(x, y) * shootSpeed;

            // 화살에 힘을 가하기
            Rigidbody2D body = arrowObj.GetComponent<Rigidbody2D>();
            body.AddForce(v, ForceMode2D.Impulse);

            // 공격 중이 아님으로 설정
            Invoke("StopAttack", shootDelay); // 비동기 함수 호출, 0.25초 뒤에 StopAttack 호출
        }
    }
    // 공격 중지
    public void StopAttack()
    {
        inAttack = false; // 공격 중이 아님으로 설정
    }
}
