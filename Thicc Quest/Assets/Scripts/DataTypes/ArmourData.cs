using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArmourData : ItemData
{
    public AffinityData affinities = new AffinityData();
    public BaseStat resistance = new BaseStat("Resistance", 0);
    public BaseStat magicResist = new BaseStat("Magic Resistance", 0);
    public ArmourData()
    {
        Init();
    }
    public virtual void Init() { }
    public override float Evaluate()
    {
        return (quality.value + resistance.value + magicResist.value) / 3;
    }

}
[System.Serializable]
public class LegArmor : ArmourData
{
    public override void Init()
    {
        subType = ItemType.leg;
    }

}
[System.Serializable]
public class FootWear : ArmourData
{
    public override void Init()
    {
        subType = ItemType.foot;
    }

}
[System.Serializable]
public class BodyArmor : ArmourData
{
    public override void Init()
    {
        subType = ItemType.body;
    }
}
[System.Serializable]
public class HeadArmor : ArmourData
{
    public override void Init()
    {
        subType = ItemType.head;
    }
}
