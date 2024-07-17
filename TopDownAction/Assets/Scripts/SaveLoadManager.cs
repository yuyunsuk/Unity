using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System.Threading.Tasks; // 비동기 작업에 사용

public class GlobalData {
    public int arrows;
    public int keys;
    public int hp;

    public GlobalData() { // 생성자
        arrows = 0;
        keys = 0;
        hp = 0;
    }
}

public class SceneData {
    public string scene;
    public List<SceneObject> objects;

    public SceneData() { // 생성자
        objects = new List<SceneObject>();
    }
}

public class SceneObject {
    public string objectName; // Scene Object 이름
    public bool isEnabled;    // 활성화 여부
}

public class SaveLoadManager : MonoBehaviour
{

    string filePathGlobal;
    string filePathScene;
    public GlobalData globalData = new GlobalData();
    public SceneData sceneData   = new SceneData();

    float checkInterval = 0.2f; // 0.2초 마다 체크
    float tempTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        // 글로벌 데이터 저장파일이 있으면 로딩 -> 글로벌 데이터 업데이트
        // 없으면 -> 기본값을 글로벌 데이터에 입력하고 파일로 저장
        filePathGlobal = Path.Combine(Application.persistentDataPath, "GlobalData.json");
        if (File.Exists(filePathGlobal))
        {
            LoadGlobalData();
            PlayerController.hp  = globalData.hp;
            ItemKeeper.hasArrows = globalData.arrows;
            ItemKeeper.hasKeys   = globalData.keys;
        }
        else {
            globalData.hp     = PlayerController.hp;
            globalData.arrows = ItemKeeper.hasArrows;
            globalData.keys   = ItemKeeper.hasKeys;
            SaveGlobalData();
        }

        // 해당 씬이름의 저장파일이 있으면 로딩 -> 씬의 구성 props 의 활성화 여부 설정
        // 없으면 -> 씬의 props 를 씬 데이터에 적용하고 활성화 여부 true
        filePathScene = Path.Combine(Application.persistentDataPath, SceneManager.GetActiveScene().name + ".json"); // SceneManager.GetActiveScene().name 현재 Scene Name
        if (File.Exists(filePathScene))
        {
            LoadSceneData();
            foreach (SceneObject obj in sceneData.objects) {
                if (!obj.isEnabled) // 비활성화
                {
                    GameObject target = GameObject.Find(obj.objectName);
                    if (target != null)
                    { // 해당 아이템이 있으면
                        if (target.GetComponent<ItemBox>() != null)
                        {
                            target.GetComponent<ItemBox>().isClosed = false;
                        }
                        else
                        {
                            target.SetActive(false);
                        }
                    }
                }
            }
        } else {
            // 씬의 props 를 씬 데이터에 적용하고 활성화 여부 true
            sceneData.scene = SceneManager.GetActiveScene().name;
            AddObjectToSceneData("Item");    // Tag 이름
            AddObjectToSceneData("ItemBox"); // Tag 이름
            AddObjectToSceneData("Door");    // Tag 이름
            SaveSceneData();
        }
    }

    // Update is called once per frame
    void Update()
    {
        tempTime += Time.deltaTime;

        if (tempTime > checkInterval) { // 1초마다 체크함
            bool dataChanaged = false;

            if (globalData.hp != PlayerController.hp) {
                globalData.hp = PlayerController.hp;
                dataChanaged = true;
            }

            if (globalData.arrows != ItemKeeper.hasArrows)
            {
                globalData.arrows = ItemKeeper.hasArrows;
                dataChanaged = true;
            }

            if (globalData.keys != ItemKeeper.hasKeys)
            {
                globalData.keys = ItemKeeper.hasKeys;
                dataChanaged = true;
            }

            if (dataChanaged) {
                SaveGlobalData();
            }

            tempTime = 0;
        }
    }

    public void LoadGlobalData() {
        string jsonData = File.ReadAllText(filePathGlobal);
        globalData      = JsonConvert.DeserializeObject<GlobalData>(jsonData);
        Debug.Log("Data loaded from " + filePathGlobal);
    }

    /* 비동기 저장을 위해 막음
    public void SaveGlobalData()
    {
        string jsonData = JsonConvert.SerializeObject(globalData, Formatting.Indented);
        File.WriteAllText(filePathGlobal, jsonData);
        Debug.Log("Data saved to  " + filePathGlobal);
    }
    */

    public async void SaveGlobalData() {
        await SaveGlobalDataAsync(); // 저장 기능은 비동기 처리함
    }

    async Task SaveGlobalDataAsync() { // Task를 리턴하는 비동기 함수
        string jsonData = JsonConvert.SerializeObject(globalData, Formatting.Indented);

        using (StreamWriter writer = new StreamWriter(filePathGlobal)) { // 메모리에 StreamWriter 할당, using 의 역할
            await writer.WriteAsync(jsonData);
        }
        Debug.Log("Data saved to " + filePathGlobal);
    }

    public void LoadSceneData()
    {
        string jsonData = File.ReadAllText(filePathScene);
        sceneData = JsonConvert.DeserializeObject<SceneData>(jsonData); // 역직렬화시 데이터 타입 조심
        Debug.Log("Data loaded from " + filePathScene);
    }

    public void SaveSceneData()
    {
        string jsonData = JsonConvert.SerializeObject(sceneData, Formatting.Indented);
        File.WriteAllText(filePathScene, jsonData);
        Debug.Log("Data saved to  " + filePathScene);
    }

    public void AddObjectToSceneData(string tag) {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objects) {
            SceneObject sceneObject = new SceneObject();
            sceneObject.objectName = obj.name;
            sceneObject.isEnabled = obj.activeSelf;

            sceneData.objects.Add(sceneObject);
        }
    }

    public void SetSceneData(string name, bool value) {

        bool dataChanged = false;

        foreach (SceneObject obj in sceneData.objects) {
            if (obj.objectName == name) {
                obj.isEnabled = value;
                dataChanged = true;
            }
        }
        if (dataChanged) {
            SaveSceneData();
        }
    }
}
