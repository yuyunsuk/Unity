using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public static int score;
    public GameObject gameOvertext;
    Text text;
    void Awake()
    {
        text = GetComponent<Text>();
        score = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Score: " + score;
        if (score > 30)
        {
            gameOvertext.SetActive(true);
        }
    }
}
