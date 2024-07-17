using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 종류
public enum ItemType
{
    arrow,  // 화살
    key,    // 열쇠
    life,   // 생명
}

public class ItemData : MonoBehaviour
{
    SaveLoadManager saveLoadManager;

    public ItemType type;       // 아이템의 종류
    public int count = 1;       // 아이템 수

    public int arrangeId = 0;   // 식별을 위한 값

    // Start is called before the first frame update
    void Start()
    {
        saveLoadManager = GameObject.FindObjectOfType<SaveLoadManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    // 접촉 (물리)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Player 와 충돌이 일어났을때
        if (collision.gameObject.tag == "Player")
        {
            // 열쇠
            if (type == ItemType.key)
            {
                // 열쇠 +1
                ItemKeeper.hasKeys += 1;
            }
            else if (type == ItemType.arrow)
            {
                // 화살 +1
                ArrowShoot shoot = collision.gameObject.GetComponent<ArrowShoot>();
                ItemKeeper.hasArrows += count;
            }
            else if (type == ItemType.life)
            {
                // 생명
                if (PlayerController.hp < 3)
                {
                    // HP 가 3이하면 추가
                    PlayerController.hp++;

                    // HP 갱신
                    PlayerPrefs.SetInt("PlayerHP", PlayerController.hp);
                }
            }
            // 배치 Id 저장
            saveLoadManager.SetSceneData(this.gameObject.name, false);

            Debug.Log("ItemKeeper.hasKeys: "   + ItemKeeper.hasKeys);
            Debug.Log("ItemKeeper.hasArrows: " + ItemKeeper.hasArrows);
            Debug.Log("ItemKeeper.life: "      + PlayerController.hp);

            //++++ 아이템 획득 연출 ++++
            // 충돌 판정 비활성
            gameObject.GetComponent<CircleCollider2D>().enabled = false;

            // 아이템의 Rigidbody2D가져오기
            Rigidbody2D itemBody = GetComponent<Rigidbody2D>();

            // 중력 젹용
            itemBody.gravityScale = 2.5f; // 1.0f 는 중력 그대로, 2.5f 는 중력의 2.5배
            //itemBody.gravityScale = 1.0f; // 1.0f 는 중력 그대로, 2.5f 는 중력의 2.5배 (안뛰어오름)

            // 위로 튀어오르는 연출
            itemBody.AddForce(new Vector2(0, 6), ForceMode2D.Impulse); // 중력이 1.0f 일때 너무 높이 올라감

            // 0.5초 뒤에 제거
            Destroy(gameObject, 0.5f);
        }
    }
}

