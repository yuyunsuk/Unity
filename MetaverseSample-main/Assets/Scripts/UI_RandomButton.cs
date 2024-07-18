using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class UI_RandomButton : MonoBehaviour
{
    public void MakeRandom()
    {
        UI_CusomizationManager.Instance.customizationManager.MakeRandom();
    }

    public void Save()
    {
        UI_CusomizationManager.Instance.customizationManager.Save();
    }

    public void Load()
    {
        UI_CusomizationManager.Instance.customizationManager.Load();
    }
}
