using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{

    private List<ItemData> items = new List<ItemData>();

    public void AddItem(ItemData i)
    {
        switch(i.type)
        {
            case ItemType.item:
                {
                    items.Add(i);
                    break;
                }
            case ItemType.weapon:
                {
                    items.Add((WeaponData)i);
                    break;
                }
            case ItemType.armour:
                {
                    items.Add((ArmourData)i);
                    break;
                }
        }
    }

    public void PrintInventory()
    {
        Debug.Log("You have: " + items.Count + "Items");
        foreach (ItemData i in items)
        {
            Debug.Log(i.name);
        }
    }

    //------------------- Weapon stuff---------\\
    private List<WeaponData> weapons = new List<WeaponData>();
    private int wIter = 0;

    private bool IsWOk()
    {
        if (weapons.Count < 1) return false;
        if (wIter > weapons.Count) wIter = 0;
        return true;
    }
    public List<WeaponData> GetWeapons() { return weapons; }
    public WeaponData GetNextWeapon()
    {
        if(IsWOk())
        {
            wIter++;
            return weapons[wIter - 1];
        }
        return null;
    }


    //----------------- Armour stuff---------------------\\
    private List<ArmourData> armour = new List<ArmourData>();
    private int aIter = 0;

    private bool IsAOk()
    {
        if (armour.Count < 1) return false;
        if (aIter > armour.Count) aIter = 0;
        return true;
    }

    public List<ArmourData> GetAllList() { return armour; }

    public ArmourData GetNextArmour()
    {
        if (IsAOk())
        {
            aIter++;
            return armour[aIter - 1];
        }
        Debug.Log("Armor List not working");
        return null;
    }

    public ArmourData GetArmour(string id)
    {
        foreach (ArmourData a in armour)
        {
            if (a.uID == id) return a;
        }
        return null;
    }

    public ArmourData GetArmourByName(string name)
    {
        foreach (ArmourData a in armour)
        {
            if (a.name == name) return a;
        }
        return null;
    }
}
