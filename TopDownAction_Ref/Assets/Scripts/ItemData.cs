using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//아이템 종류
public enum ItemType
{
    arrow,      //화살
    key,        //열쇠
    life,	   //생명
}

public class ItemData : MonoBehaviour
{
    public ItemType type;           //아이템의 종류
    public int count = 1;           //아이템 수

    public int arrangeId = 0;       //식별을 위한 값

    SaveLoadManager saveLoadManager;

    // Start is called before the first frame update
    void Start()
    {
        saveLoadManager = GameObject.FindObjectOfType<SaveLoadManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    //접촉 (물리)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (type == ItemType.key)
            {
                //열쇠
                ItemKeeper.hasKeys += 1;
            }
            else if (type == ItemType.arrow)
            {
                //화살
                ArrowShoot shoot = collision.gameObject.GetComponent<ArrowShoot>();
                ItemKeeper.hasArrows += count;
            }
            else if (type == ItemType.life)
            {
                //생명
                if (PlayerController.hp < 3)
                {
                    //HP가 3이하면 추가
                    PlayerController.hp++;
                }
            }
            saveLoadManager.SetSceneData(this.gameObject.name, false);

            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            //아이템의 Rigidbody2D가져오기
            Rigidbody2D itemBody = GetComponent<Rigidbody2D>();
            //중력 적용
            itemBody.gravityScale = 2.5f;
            //위로 튀어오르는 연출
            itemBody.AddForce(new Vector2(0, 6), ForceMode2D.Impulse);
            //0.5초 뒤에 제거
            Destroy(gameObject, 0.5f);
        }
    }
}
