using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LevelItem : MonoBehaviour
{
    public GameObject prefab; 

    public void OnClick()
    {
        if(prefab != null)
        {
            CreateLevelManager.instance.SelectItem(prefab);
        }
    }
}
