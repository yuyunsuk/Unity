using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{

    public GameObject otherTarget;

    // static 변수
    public static int doorNumber = 0;   // 문 번호

    // PreFab 으로 등록해서 위치 이동
    public GameObject PlayerPrefab;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {

        // 위치 벡터
        Vector3 pos = transform.position;

        // PreFab 으로 등록해서 위치 이동
        player = Instantiate(PlayerPrefab, pos, Quaternion.identity);

        // 플레이어 캐릭터 위치
        // 출입구를 배열로 얻기
        GameObject[] enters = GameObject.FindGameObjectsWithTag("Exit");

        for (int i = 0; i < enters.Length; i++)
        {
            GameObject doorObj = enters[i];           // 배열에서 꺼내기
            Exit exit = doorObj.GetComponent<Exit>(); // Exit 클래스 변수

            if (doorNumber == exit.doorNumber)
            {
                //==== 같은 문  번호 ====
                //플레이어 캐릭터를 출입구로 이동
                float x = doorObj.transform.position.x;
                float y = doorObj.transform.position.y;

                if (exit.direction == ExitDirection.up)
                {
                    y += 1;
                }
                else if (exit.direction == ExitDirection.right)
                {
                    x += 1;
                }
                else if (exit.direction == ExitDirection.down)
                {
                    y -= 1;
                }
                else if (exit.direction == ExitDirection.left)
                {
                    x -= 1;
                }

                // GameObject player = GameObject.FindGameObjectWithTag("Player");


                    
                    GameObject.FindGameObjectWithTag("Player");
                player.transform.position = new Vector3(x, y);

                break; // 반복문 빠나오기
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (otherTarget != null)
        {
            Vector2 pos = Vector2.Lerp(player.transform.position, otherTarget.transform.position, 0.5f); // Lerp 선형보간, 중간값 계산, 부드러운 움직임에 많이 쓰임
            transform.position = new Vector3(pos.x, pos.y, -10);
        }
        else
        {
            transform.position = new Vector3(
                player.transform.position.x,
                player.transform.position.y,
                -10);
        }
    }

    // 씬 이동
    public static void ChangeScene(string scnename, int doornum)
    {
        doorNumber = doornum; // 문 번호를 static 변수에 저장

        // string nowScene = PlayerPrefs.GetString("LastScene");
        // if (nowScene != "")
        // {
        //    SaveDataManager.saveArrangeData(nowScene);   // 배치 데이터 저장
        // }
        // PlayerPrefs.SetString("LastScene", scnename);   // 장면 이름 저장
        // PlayerPrefs.SetInt("LastDoor", doornum);        // 문 번호 저장
        // ItemKeeper.SaveItem();                          // 항목 저장

        SceneManager.LoadScene(scnename); // 씬 이동
    }
}
