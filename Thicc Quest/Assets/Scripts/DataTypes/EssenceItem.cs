using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EssenceItem
{
    public AffType type;
    public int quantity;
    private bool colGot = false;
    private Color c;
    public Color color
        {
        set { c = value; }
        get
        {
            if (AssetManager.Instance != null &&!colGot)
            {
                c = AssetManager.Instance.GetAffColor(type);
                colGot = true;
            }
            c.a = 1;
            return c;
        }
        }
    public Color EditCol(int index, float val)
    {
        Color c = color;
        switch (index)
        {
            case 0:
                c.r = val;
                break;
            case 1:
                c.g = val;
                break;
            case 2:
                c.b = val;
                break;
            case 3:
                c.a = val;
                break;

        }
        return c;
    }
    public EssenceItem(AffType t, int q)
    {
        type = t;
        quantity = q;
    }
    public void Decrement()
    {
        quantity--;
    }
    public void Increment()
    {
        quantity++;
    }
}
