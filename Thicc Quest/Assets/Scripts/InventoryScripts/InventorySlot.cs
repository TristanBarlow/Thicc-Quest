using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventorySlot
{
    private ItemData id;
    public GameObject obj;
    public Image image;

    public void Init(ItemData i)
    {
        obj.SetActive(true);
        image.sprite = i.sprite;
        id = i;
    }
    public ItemData GetData() { return id; }
    public void Reset(Sprite def)
    {
        image.sprite = def;
        obj.SetActive(false);
        id = null;
    }
}

