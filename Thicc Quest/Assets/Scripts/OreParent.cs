using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum OreType
{
    Magic,
    Dark,
    Orc,
    Frost,
    Fire,
    Steel
}

[System.Serializable]
public class OreParent
{
    public OreType type;
    public int quantity;
    public Color col;
    public OreParent(OreType t, int q, Color c)
    {
        type = t;
        quantity = q;
        col = c;
    }
}
