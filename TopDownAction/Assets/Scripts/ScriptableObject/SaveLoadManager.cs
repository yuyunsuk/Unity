using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

#if USE_SCRIPTABLE_OBJECT // 전처리 지시어 #if, #endif, #elif 등
public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance { get; private set; } // SaveLoadManager 첫번째 위치 저장, Singleton 패턴

    public GlobalData globalData;
    public SceneData sceneData;

    float checkInterval = 0.2f;
    float tempTime = 0;
    string globalDataPath;
    string sceneDataPath;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Load 중에 제거 방지
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Debug.Log("Game started");
        globalDataPath = Path.Combine(Application.persistentDataPath, "GlobalData.json");
        if (File.Exists(globalDataPath))
        {
            // json파일이 있으면 ScriptableObject의 값 업데이트
            globalData = LoadData<GlobalData>(globalDataPath); // Start 시에 Load 1
            PlayerController.hp = globalData.hp;
            ItemKeeper.hasArrows = globalData.arrows;
            ItemKeeper.hasKeys = globalData.keys;
        }   
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded: {scene.name}");
        sceneData = Resources.Load<SceneData>($"Record/{scene.name}Data");
        if (sceneData == null)
        {
            return;
        }
        sceneDataPath = Path.Combine(Application.persistentDataPath, scene.name + ".json");
        if (File.Exists(sceneDataPath))
        {
            sceneData = LoadData<SceneData>(sceneDataPath); // Scene 로드시에 Load 2
            foreach (SceneObject obj in sceneData.objects)
            {
                GameObject target = GameObject.Find(obj.objectName);
                if (target != null)
                {
                    if (!obj.isEnabled)
                    {
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
        }
        else
        {
            sceneData.objects.Clear();
            AddObjectsToPropsData("ItemBox");
            AddObjectsToPropsData("Door");
            AddObjectsToPropsData("Item");
        }            
    }

    void OnSceneUnloaded(Scene scene) // Scene 넘어갈때 Unloaded 마다 Save 1
    {
        Debug.Log($"Scene unloaded: {scene.name}");
        SaveData<SceneData>(sceneData, sceneDataPath);
        SaveData<GlobalData>(globalData, globalDataPath);
    }
    
    void Update()
    {
        tempTime += Time.deltaTime;
        if (tempTime > checkInterval)
        {
            if (globalData.hp != PlayerController.hp)
            {
                globalData.hp = PlayerController.hp;
            }
            if (globalData.arrows != ItemKeeper.hasArrows)
            {
                globalData.arrows = ItemKeeper.hasArrows;
            }
            if (globalData.keys != ItemKeeper.hasKeys)
            {
                globalData.keys = ItemKeeper.hasKeys;
            }
            tempTime = 0;
        }
    }

    public T LoadData<T>(string filePath)
    {
        string jsonData = File.ReadAllText(filePath);
        T data = JsonConvert.DeserializeObject<T>(jsonData); 
        Debug.Log("Data loaded from " + filePath);
        return data;
    }

    public void SaveData<T>(T data, string filePath)
    {
        string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(filePath, jsonData);
        Debug.Log("Data saved to " + filePath);
    }

    void AddObjectsToPropsData(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objects)
        {
            SceneObject sceneObject = new SceneObject();
            sceneObject.objectName = obj.name;
            sceneObject.isEnabled = obj.activeSelf;            
            sceneData.objects.Add(sceneObject);
        }
    }

    public void SetSceneData(string name, bool value)
    {
        foreach (SceneObject obj in sceneData.objects)
        {
            if (obj.objectName == name)
            {
                obj.isEnabled = value;
            }
        }
    }

    void OnDestroy() // 종료 전에 호출됨
    {
        // 이벤트 등록 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    void OnApplicationQuit()
    {
        Debug.Log("Game ended"); // 런타임 중에는 Singleton 계속 기억하고 있기 때문에, 마지막에 저장, Scene 넘어갈때 마다 Save 2
        SaveData<SceneData>(sceneData, sceneDataPath);
        SaveData<GlobalData>(globalData, globalDataPath);
    }
}
#endif