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
    earth, air,  dark, light, magic, spirtual, frost, fire, none
}

[System.Serializable]
public class BaseAffinityData
{
    public AffType type;
    public string title;
    public List<AffType> Weak;
    public List<AffType> Strong;
    public UnityEngine.Color col;
}

[System.Serializable]
public class AffinityData
{
    private List<Affinity> affinities = new List<Affinity>();

    public AffinityData()
    {
        affinities.Add(new Affinity(AffType.earth, 0.0f));
        affinities.Add(new Affinity(AffType.air, 0.0f));
        affinities.Add(new Affinity(AffType.dark, 0.0f));
        affinities.Add(new Affinity(AffType.light, 0.0f));
        affinities.Add(new Affinity(AffType.magic, 0.0f));
        affinities.Add(new Affinity(AffType.spirtual, 0.0f));
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
    public List<Affinity> GetAffinities() { return affinities; }

    public float AffinityEvaluation()
    {
        float total = 0;
        foreach (Affinity a in affinities)
        {
            total += a.value;
        }
        return total / affinities.Count;
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
