using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArmourData : ItemData
{
    public AffinityData affinities = new AffinityData();
    public BaseStat resistance = new BaseStat("Resistance", 0);
    public BaseStat magicResist = new BaseStat("Magic Resistance", 0);
    ArmourData() { type = ItemType.armour; }
    public override float Evaluate()
    {
        return (quality.value + resistance.value + magicResist.value) / 3;
    }
}
