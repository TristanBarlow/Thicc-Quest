using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml.Serialization;
using System.Xml;
using System.Runtime.Serialization;

[System.Serializable]
public enum AffType
{
    earth, dark, magic, orc, frost, fire, none
}

[System.Serializable]
public class AffinityData
{
    private List<Affinity> affinities = new List<Affinity>();

    public AffinityData()
    {
        affinities.Add(new Affinity(AffType.earth, 0.0f));
        affinities.Add(new Affinity(AffType.dark, 0.0f));
        affinities.Add(new Affinity(AffType.magic, 0.0f));
        affinities.Add(new Affinity(AffType.orc, 0.0f));
        affinities.Add(new Affinity(AffType.frost, 0.0f));
        affinities.Add(new Affinity(AffType.fire, 0.0f));
    }
    public void ChangeAffinityValue(AffType aff, float delta)
    {
        GetAffValue(aff).value += delta;
    }

    public Affinity GetAffValue(AffType t)
    {
        foreach (Affinity aff in affinities)
        {
            if (aff.type == t) return aff;
        }
        return new Affinity(AffType.none, 0);
    }

}

[System.Serializable]
public class Affinity
{
    public AffType type = AffType.earth;
    public float value = 0.0f;

    public Affinity(AffType t, float va)
    {
        type = t;
        value = va;
    }
}
