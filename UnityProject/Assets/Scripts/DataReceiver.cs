using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //        "lectureId": "L00000000052",
    //        "lectureContentSeq": 3,
    //        "questionSeq": 1,
    //        "question": "다음 중 HTML5의 시맨틱 태크 (Semantic Tag)가 아닌 것은?",
    //        "answer1": "head",
    //        "answer2": "nav",
    //        "answer3": "aside",
    //        "answer4": "footer",
    //        "correctAnswer": 1

}

public class DataReceiver : MonoBehaviour
{

    public void ReceiveJsonData(string jsonData)
    {
        // JSON 문자열을 C# 객체 배열로 변환
        MyData[] dataArray = JsonUtility.FromJson<MyDataArrayWrapper>(jsonData).items;

        // 데이터 처리 로직
        if (dataArray != null && dataArray.Length > 0)
        {
            foreach (var data in dataArray)
            {
                Debug.Log($"Received data: Question = {data.question}, Answer1 = {data.answer1}");
            }
        }
        else
        {
            Debug.Log("No data received or data is null.");
        }
    }

    [System.Serializable]
    public class MyDataArrayWrapper
    {
        public MyData[] items;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
