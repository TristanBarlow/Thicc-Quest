using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using UnityEngine;

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
    public Color col;

    public bool IsColorSimilar(float threshold, Color compCol)
    {
        if (col == null) return false;

        Vector4 difference = col - compCol;

        float abs = Mathf.Abs(difference.magnitude);

        return abs < threshold;
    }

    public float ColorDifference(Color compCol)
    {
        if (col == null) return 0;

        Vector4 difference = col - compCol;

        float abs = Mathf.Abs(difference.magnitude);

        return abs;
    }

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
    public void SetAffinityValue(AffType aff, float val)
    {
        GetAffValue(aff).value = val;
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

    public void PrintMe()
    {
        string str = "";
        foreach (Affinity aff in affinities)
        {
            str += aff.type + ": " + aff.value+ "\n";
        }
        Debug.Log(str);
    }

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
