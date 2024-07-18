using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CreateLevelManager : MonoBehaviour
{
    public static CreateLevelManager instance; // �̱���
   
    Transform selectItem = null;    // ���õ� ���� ������    

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

        // ��ŷ ��ġ�� �����´�. ������Ʈ�� ��ġ�� �����Ѵ�.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit) ) 
        {
            selectItem.position = hit.point; 
        }        

        // �¿� ȸ�� 
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

        // ���� ��ư Ŭ���� �װ��� ��ġ�Ѵ�
        if(Input.GetMouseButtonDown(0)) 
        {
            selectItem.gameObject.layer = 0; // �浹�� ���ش�
            selectItem.GetComponent<Collider>().enabled = true;
            selectItem = null; 
        }
    }

    // UI���� �������� ���������� ȣ��Ǵ� �Լ�
    public void SelectItem(GameObject itemPrefab)
    {
        GameObject newItem = Instantiate(itemPrefab);
        selectItem = newItem.transform;

        // 180�� ȸ�� �����ش�.
        selectItem.rotation = Quaternion.Euler(0, 180, 0); 

        // ��ŷ �浹�� ���ش�.
        newItem.layer = 2; 
        newItem.GetComponent<Collider>().enabled = false;
    }
}
