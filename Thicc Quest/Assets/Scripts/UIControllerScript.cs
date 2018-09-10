using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControllerScript : MonoBehaviour {

    public static UIControllerScript Instance { set; get; }
    public GameObject[] menus;

    private void Start()
    {
        Instance = this;
    }
    public void HideAllMenus()
    {
        foreach (GameObject obj in menus)
        {
            obj.SetActive(false);
        }
    }

    public void SetMenuActive(int index)
    {
        HideAllMenus();
        menus[index].SetActive(true);
    }
}
