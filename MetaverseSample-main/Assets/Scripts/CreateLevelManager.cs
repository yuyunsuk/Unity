using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CreateLevelManager : MonoBehaviour
{
    public static CreateLevelManager instance; // 싱글톤
   
    Transform selectItem = null;    // 선택된 레벨 아이템    

    // Start is called before the first frame update
    void Start()
    {
        instance = this;             
    }

    // Update is called once per frame
    void Update()
    {
        PlaceGameObject();
    }

    void PlaceGameObject()
    {
        if (selectItem == null)
            return; 

        // 픽킹 위치를 가져온다. 오브젝트의 위치를 변경한다.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit) ) 
        {
            selectItem.position = hit.point; 
        }        

        // 좌우 회전 
        if(Input.GetKeyUp(KeyCode.Q))
        {
            float yaw = selectItem.rotation.eulerAngles.y;
            selectItem.rotation = Quaternion.Euler(0, yaw + 90, 0); 
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            float yaw = selectItem.rotation.eulerAngles.y;
            selectItem.rotation = Quaternion.Euler(0, yaw - 90, 0);
        }

        // 왼쪽 버튼 클릭시 그곳에 배치한다
        if(Input.GetMouseButtonDown(0)) 
        {
            selectItem.gameObject.layer = 0; // 충돌을 켜준다
            selectItem.GetComponent<Collider>().enabled = true;
            selectItem = null; 
        }
    }

    // UI에서 아이템을 선택했을때 호출되는 함수
    public void SelectItem(GameObject itemPrefab)
    {
        GameObject newItem = Instantiate(itemPrefab);
        selectItem = newItem.transform;

        // 180도 회전 시켜준다.
        selectItem.rotation = Quaternion.Euler(0, 180, 0); 

        // 픽킹 충돌을 꺼준다.
        newItem.layer = 2; 
        newItem.GetComponent<Collider>().enabled = false;
    }
}
