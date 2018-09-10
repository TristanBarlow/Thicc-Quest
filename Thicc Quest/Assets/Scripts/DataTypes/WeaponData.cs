using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponData : ItemData
{
    public BaseStat damage = new BaseStat("Damage", 0);
    public BaseStat speed = new BaseStat("Speed", 0);

    public AffinityData affinities = new AffinityData();

    public WeaponData()
    {
        type = ItemType.weapon;
    }

    public WeaponData(string n, Sprite s, string sID, int d, AffinityData aff)
    {
        name = n;
        sprite = s;
        spriteID = sID;
        damage.value = d;

        affinities = aff;
        type = ItemType.weapon;
    }
    public override float Evaluate()
    {
        return (damage.value + speed.value + quality.value + affinities.AffinityEvaluation()) / 4;
    }
}
