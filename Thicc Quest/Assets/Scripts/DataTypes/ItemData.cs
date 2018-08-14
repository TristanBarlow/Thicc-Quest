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
public class BaseStat
{
    public string title = "none";
    public int value = 0;
    public BaseStat(string s, int v) { title = s; value = v; }
}

[System.Serializable]
public class ItemData
{
    public ItemType  type = ItemType.item;
    public string name =  "";
    public string uID = "";
    public string spriteID = "";
    public int Quantity = 0;
    public BaseStat weight = new BaseStat("Weight", 0);
    public BaseStat quality = new BaseStat("Quality", 0);
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
               spr =  WeaponManager.Instance.GetSpriteFromId(spriteID);
            }
            return spr;
        }
    }
    public virtual float Evaluate()
    {
        return quality.value;
    }
}

