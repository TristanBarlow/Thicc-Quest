using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ItemType
{
    item, weapon, armour
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
    public Sprite sprite;
}

