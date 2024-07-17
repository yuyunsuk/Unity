using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    int hasKeys = 0;                   // ���� ��
    int hasArrows = 0;                 // ȭ�� ���� ��
    int hp = 0;                        // �÷��̾��� HP
    public GameObject arrowText;       // ȭ���� ���� ǥ���ϴ� Text
    public GameObject keyText;         // ���� ���� ǥ���ϴ�Text
    public GameObject hpImage;         // HP�� ���� ǥ���ϴ� Image
    public Sprite life3Image;          // HP3 �̹���
    public Sprite life2Image;          // HP2 �̹���
    public Sprite life1Image;          // HP1 �̹���
    public Sprite life0Image;          // HP0 �̹���
    public GameObject mainImage;       // �̹����� ������ GameObject
    public GameObject resetButton;     // ���� ��ư
    public Sprite gameOverSpr;         // GAME OVER �̹���
    public Sprite gameClearSpr;        // GAME CLEAR �̹���
    public GameObject inputPanel;      // ���߾� �е�� ���� ��ư�� ��ġ�� ���� �г�

    public string retrySceneName = ""; // ��õ��ϴ� �� �̸�

    // Start is called before the first frame update
    void Start()
    {
        UpdateItemCount();             // ������ �� ����
        UpdateHP();                    // HP����

        // �̹����� �����
        Invoke("InactiveImage", 1.0f);
        resetButton.SetActive(false);  // ��ư �����
    }

    // Update is called once per frame
    void Update()
    {
        UpdateItemCount();  // ������ �� ����
        UpdateHP();         // HP ����
    }

    // ������ �� ����
    void UpdateItemCount()
    {
        // ȭ��
        if (hasArrows != ItemKeeper.hasArrows)
        {
            arrowText.GetComponent<Text>().text = ItemKeeper.hasArrows.ToString();
            hasArrows = ItemKeeper.hasArrows;
        }
        // ����
        if (hasKeys != ItemKeeper.hasKeys)
        {
            keyText.GetComponent<Text>().text = ItemKeeper.hasKeys.ToString();
            hasKeys = ItemKeeper.hasKeys;
        }
    }

    // HP ����
    void UpdateHP()
    {
        // Player ��������
        if (PlayerController.gameState != "gameend")
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                if (PlayerController.hp != hp)
                {
                    hp = PlayerController.hp;
                    if (hp <= 0)
                    {
                        hpImage.GetComponent<Image>().sprite = life0Image;
                        // �÷��̾� ���!!
                        resetButton.SetActive(true);            // ��ư ǥ��
                        mainImage.SetActive(true);              // �̹��� ǥ��
                                                                // �̹��� ����
                        mainImage.GetComponent<Image>().sprite = gameOverSpr;
                        inputPanel.SetActive(false);            // ���� UI �����
                        PlayerController.gameState = "gameend"; // ���� ����
                    }
                    else if (hp == 1)
                    {
                        hpImage.GetComponent<Image>().sprite = life1Image;
                    }
                    else if (hp == 2)
                    {
                        hpImage.GetComponent<Image>().sprite = life2Image;
                    }
                    else
                    {
                        hpImage.GetComponent<Image>().sprite = life3Image;
                    }
                }
            }
        }
    }

    // ��õ�
    public void Retry()
    {
        // HP �ǵ�����
        PlayerPrefs.SetInt("PlayerHP", 3);
        // ���� ������ ����
        SceneManager.LoadScene(retrySceneName); // �� �̵�

        ItemKeeper.hasArrows = 3;
        PlayerController.hp  = 3;
        ItemKeeper.hasKeys   = 0;
    }

    // �̹��� �����
    void InactiveImage()
    {
        mainImage.SetActive(false);
    }

    // ���� Ŭ����
    public void GameClear()
    {
        // �̹��� ǥ��
        mainImage.SetActive(true);
        mainImage.GetComponent<Image>().sprite = gameClearSpr; // GAME CLEAR ����
        // ���� ui �����
        inputPanel.SetActive(false);
        // ���� Ŭ����� ǥ��
        PlayerController.gameState = "gameclear";
        // 3�ʵڿ� ���� Ÿ��Ʋ�� ���� ����
        Invoke("GoToTitle", 3.0f);
    }

    // Ÿ��Ʋ ȭ������ �̵�
    void GoToTitle()
    {
        PlayerPrefs.DeleteKey("LastScene"); // ���� ���� ����
        SceneManager.LoadScene("Title");    // Ÿ��Ʋ�� ���ư���
    }
}
