using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ItemType
{
    item, weapon, armour, all
}

[System.Serializable]
public class ItemData
{
    public ItemType  type = ItemType.item;
    public string name =  "";
    public string uID = "";
    public string spriteID = "";
    public int Quantity = 0;
    [NonSerialized]
    private Sprite spr;
    public Sprite sprite
    {
        set
        {
            spr = value;
        }
        get
        {
            if (spr == null)
            {
               spr =  AssetManager.Instance.GetSpriteFromId(spriteID);
            }
            return spr;
        }
    }
}

