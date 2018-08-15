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
    private string title = "none";
    public string Title { set { title = value; }get { return title; } }
    public int value = 0;
    public BaseStat(string s, int v) { Title = s; value = v; }
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
                switch (type)
                {
                    case ItemType.weapon:
                        spr = WeaponManager.Instance.GetSpriteFromId(spriteID);
                        break;
                }
            }
            return spr;
        }
    }
    public virtual float Evaluate()
    {
        return quality.value;
    }
}

