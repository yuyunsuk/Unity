using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Component;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public float moveSpeedSet = 4f;
    float moveSpeed;
    float SpeedKeep;
    public bool LockRotation = false;
    float timer = 0.0f;
    float Damagedtimer = 0.0f;
    float waitingTime = 0.1f;
    private Vector3 Move;
    private Rigidbody Rigid;

    string sceneName;

    Animator animator;
    GameObject sceneStatusManager;
    public string stopAnime = "Idle";
    public string moveAnime = "Walk";
    public string runAnime = "Run";
    string nowAnime = "";
    string oldAnime = "";

    bool isObstacle = false;

    public GameObject runningEffect;
    public Material playerMaterial;
    CameraShake Camera;

    [System.Serializable]
    public class MyData
    {
        public string lectureId;
        public long lectureContentSeq;
        public int questionSeq;
        public string question;
        public string answer1;
        public string answer2;
        public string answer3;
        public string answer4;
        public int correctAnswer;
    }

    [System.Serializable]
    public class MyDataArrayWrapper
    {
        public MyData[] items;
    }

    public void ReceiveJsonData(string jsonData)
    {

        Debug.Log("Unity Received JSON: " + jsonData);

        // JSON 문자열을 MyDataArrayWrapper 객체로 변환
        var wrapper = JsonUtility.FromJson<MyDataArrayWrapper>("{\"items\":" + jsonData + "}");

        // 래퍼에서 MyData 배열을 추출
        MyData[] dataArray = wrapper.items;

        // 데이터 처리 로직
        if (dataArray != null && dataArray.Length > 0)
        {
            foreach (var data in dataArray)
            {
                Debug.Log($"Received data: lectureId = {data.lectureId}, lectureContentSeq = {data.lectureContentSeq}, questionSeq = {data.questionSeq}," +
                    $" Question = {data.question}, Answer1 = {data.answer1}, Answer2 = {data.answer2}, Answer3 = {data.answer3}, Answer4 = {data.answer4}, correctAnswer = {data.correctAnswer}");
            }
        }
        else
        {
            Debug.Log("No data received or data is null.");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Rigid = this.GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        sceneStatusManager = GameObject.FindGameObjectWithTag("SceneStatus");
        nowAnime = stopAnime;
        oldAnime = stopAnime;
        moveSpeed = moveSpeedSet;
        sceneName = SceneManager.GetActiveScene().name;
        SpeedKeep = moveSpeedSet;
        Camera = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneStatusManager.GetComponent<SceneStatusManager>().sceneState == SceneStatusManager.SceneStatus.Ready)
        {
            return;
        }

        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) || sceneName == "RunningMap")
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                transform.rotation = Quaternion.Euler(0, 315, 0);
                LockRotation = true;
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                transform.rotation = Quaternion.Euler(0, 45, 0);
                LockRotation = true;
            }
            else if (!LockRotation)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && sceneName != "RunningMap")
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                transform.rotation = Quaternion.Euler(0, 225, 0);
                LockRotation = true;
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                transform.rotation = Quaternion.Euler(0, 135, 0);
                LockRotation = true;
            }
            else if (!LockRotation)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        else if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && !LockRotation)
        {
            transform.rotation = Quaternion.Euler(0, 270, 0);
        }
        else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && !LockRotation)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }

    private void FixedUpdate()
    {
        if (nowAnime == runAnime)
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            Instantiate(runningEffect, pos, Quaternion.Euler(0, transform.rotation.y, 0));
        } 

        if (isObstacle)
        {
            moveSpeedSet = 2;
            Color color;
            ColorUtility.TryParseHtmlString("#FF7272", out color);
            playerMaterial.color = color;
            Damagedtimer += Time.deltaTime;
            if (Damagedtimer > 0.5f)
            {

                playerMaterial.color = Color.white;
                moveSpeedSet = SpeedKeep;
                isObstacle = false;
                Damagedtimer = 0.0f;
            }
        }

        if (sceneStatusManager.GetComponent<SceneStatusManager>().sceneState == SceneStatusManager.SceneStatus.Ready)
        {
            return;
        }

        if (Input.GetKey(KeyCode.LeftShift) || sceneName == "RunningMap")
        {
            moveSpeed = (moveSpeedSet * 2);
        }
        else if (!Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = moveSpeedSet;
        }

        float moveDistanceZ = Input.GetAxisRaw("Vertical");
        if (sceneName == "RunningMap")
        {
            moveDistanceZ = 1.0f;
        }
        float moveDistanceX = Input.GetAxisRaw("Horizontal");

        Move = new Vector3(moveDistanceX, 0, moveDistanceZ);

        Rigid.velocity = Move.normalized * moveSpeed;

        if (LockRotation)
        {
            timer += Time.deltaTime;
            if (timer > waitingTime)
            {
                LockRotation = !LockRotation;
                timer = 0.0f;
            }
        }

        if (moveDistanceX == 0.0f && moveDistanceZ == 0.0f)
        {
            nowAnime = stopAnime;
        }
        else if (moveSpeed == (moveSpeedSet * 2))
        {
            nowAnime = runAnime;
        }
        else if (moveSpeed == moveSpeedSet)
        {
            nowAnime = moveAnime;
        }
        if (nowAnime != oldAnime)
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime);
        }
    }
      
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            Camera.VibrateForTime(0.2f);
            isObstacle = true;
            Debug.Log("OnTriggerEnter Obstacle!!!");
        }
        if (collision.gameObject.tag == "Wall")
        {
            isObstacle = true;
            Rigid.AddForce(new Vector3(transform.position.x * 3, 0, transform.position.z * -3), ForceMode.VelocityChange);
            Debug.Log("OnTriggerEnter Wall!!!");
        }
    }
}

