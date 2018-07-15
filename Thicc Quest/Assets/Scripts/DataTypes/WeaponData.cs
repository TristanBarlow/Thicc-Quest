using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponData : ItemData
{
    public float dmg = 0.0f;
    AffinityData affinity = new AffinityData();

    public WeaponData()
    { type = ItemType.weapon; }

    public WeaponData(string n, Sprite s, string sID, float d, AffinityData aff)
    {
        name = n;
        sprite = s;
        spriteID = sID;
        dmg = d;
        affinity = aff;
        type = ItemType.weapon;
    }
}
