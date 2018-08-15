using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : ISave
{
    public void LoadFailed()
    {
        throw new NotImplementedException();
    }

    public void LoadSuccess(object obj)
    {
        throw new NotImplementedException();
    }
}

[System.Serializable]
public class EquipmentSlot
{
    ItemType slotType;
    Sprite sprite;
    ItemData itemData;
    SpriteRenderer rend;

    public EquipmentSlot(SpriteRenderer rend, ItemData id, ItemType sType)
    {
        slotType = sType;
        if (slotType == id.type)
        {
            itemData = id;
        }
        else id = null;
  
    }
}

