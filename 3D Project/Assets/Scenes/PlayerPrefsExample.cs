using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsExample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("PlayerScore"))
        {
            // ���� ����
            PlayerPrefs.SetInt("PlayerScore", 100);
        }
        // ���� �ε�
        int score = PlayerPrefs.GetInt("PlayerScore");

        if (!PlayerPrefs.HasKey("PlayerDefense"))
        {
            // �Ǽ� ����
            PlayerPrefs.SetFloat("PlayerDefense", 75.5f);
        }
        // �Ǽ� �ε�
        float defense = PlayerPrefs.GetFloat("PlayerDefense");

        if (!PlayerPrefs.HasKey("PlayerName"))
        {
            // ���ڿ� ����
            PlayerPrefs.SetString("PlayerName", "Player");
        }
        // ���ڿ� �ε�
        string name = PlayerPrefs.GetString("PlayerName");

        Debug.Log("PlayerScore: "   + score);
        Debug.Log("PlayerDefense: " + defense);
        Debug.Log("PlayerName: "    + name);

        //// Ű ����
        //PlayerPrefs.DeleteKey("PlayerScore");
        //PlayerPrefs.DeleteKey("PlayerDefense");
        //PlayerPrefs.DeleteKey("PlayerName");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
