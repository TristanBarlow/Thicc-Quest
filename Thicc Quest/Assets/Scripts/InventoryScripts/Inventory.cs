using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory:ISave
{

    private List<ItemData> items = new List<ItemData>();

    int wIter= 0, iIter= 0, aIter = 0, nIter = 0;

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
        MessageManager.Instance.NewMessage("The " + i.type + " " + i.name + " has been added to you inventory");
    }

    public void PrintInventory()
    {
        Debug.Log("You have: " + items.Count + "Items");
        foreach (ItemData i in items)
        {
            Debug.Log(i.name);
        }
    }

    public List<ItemData> GetAllItemsOfType(ItemType type)
    {
        List<ItemData> list = new List<ItemData>();
        foreach (ItemData id in items)
        {
            if (id.type == type || type == ItemType.all) list.Add(id);
        }
        return list;
    }

    public List<ItemData> GetItemDataOfType(int number, ItemType type, bool lastSet = false)
    {
        List<ItemData> its = new List<ItemData>();
        List<ItemData> AllOfType = GetAllItemsOfType(type);
        int cIter = GetCurrentIter(type);

        int numPages = (int)(AllOfType.Count / number);
        int remainder = AllOfType.Count % number;
        int startIter = 0;

        if (lastSet) cIter--;
        else cIter++;

        if (cIter > numPages)
        {
            cIter = 0;
            startIter = 0;
        }
        else if (cIter < 0)
        {
            startIter = AllOfType.Count - remainder;
            cIter = numPages;
        }
        else
        {
            startIter = cIter * number;
        }

        while (startIter < AllOfType.Count  &&  its.Count< number)
        {
            its.Add(AllOfType[startIter]);
            startIter++;
        }
        SetCurrentIter(cIter, type);
        return its;
    }

    public bool RemoveItem(ItemData i)
    {
        if (items.Contains(i))
        {
            items.Remove(i);
            
            return true;
        }
        else return false;
    }

    public bool RemoveItemByID(string id)
    {
        for(int i = 0; i < items.Count; i ++) 
        {
            ItemData item = items[i];
            if (item.name == id)
            {
                items.Remove(item);
                return true;
            }
        }
        return false;
    }

    public void LoadSuccess(object obj)
    {
        Inventory i = (Inventory)obj;
        items = i.items;
    }

    public void LoadFailed()
    {
        Debug.Log("Failed Inventory load");
        items = new List<ItemData>();
    }

    public void SetCurrentIter(int newVal, ItemType iterType)
    {
        switch (iterType)
        {
            case ItemType.armour:
                aIter = newVal;
                break;
            case ItemType.weapon:
                wIter = newVal;
                break;
            case ItemType.item:
                iIter = newVal;
                break;
            case ItemType.all:
                nIter = newVal;
                break;
        }
    }
    public int GetCurrentIter(ItemType iterType)
    {
        switch (iterType)
        {
            case ItemType.armour:
                return aIter;
            case ItemType.weapon:
                return wIter;
            case ItemType.item:
                return iIter;
            case ItemType.all:
                return nIter;
        }
        return 0;
    }
}
