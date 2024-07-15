using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public GameObject startButton;      // ��ŸƮ ��ư
    public GameObject continueButton;   // �̾��ϱ� ��ư

    Button startBtn;
    Button continueBtn;

    // public string firstSceneName;       // ���� ���� �� �̸�

    // Start is called before the first frame update
    void Start()
    {
        startBtn = startButton.GetComponent<Button>();
        startBtn.onClick.AddListener(StartButtonClicked);

        continueBtn = continueButton.GetComponent<Button>();
        continueBtn.onClick.AddListener(ContinueButtonClicked);

        //string sceneName = PlayerPrefs.GetString("LastScene");      //���� �� ��
        //if (sceneName == "")
        //{
        //    continueButton.GetComponent<Button>().interactable = false; //��Ȱ��
        //}
        //else
        //{
        //    continueButton.GetComponent<Button>().interactable = true; //Ȱ��
        //}
        ////Ÿ��Ʋ BGM ���
        //SoundManager.soundManager.PlayBgm(BGMType.Title);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //��ŸƮ ��ư ����
    public void StartButtonClicked()
    {
        ////���� �����͸� ����
        //PlayerPrefs.DeleteAll();
        ////HP �ʱ�ȭ 
        //PlayerPrefs.SetInt("PlayerHP", 3);
        ////����� �������� ������ ����
        //PlayerPrefs.SetString("LastScene", firstSceneName); //�� �̸� �ʱ�ȭ
        //RoomManager.doorNumber = 0;

        SceneManager.LoadScene("WorldMap");
    }

    //�̾��ϱ� ��ư ����
    public void ContinueButtonClicked()
    {
        string sceneName = PlayerPrefs.GetString("LastScene");      //����� ��
        RoomManager.doorNumber = PlayerPrefs.GetInt("LastDoor");    //�� ��ȣ
        SceneManager.LoadScene("WorldMap");
    }
}

