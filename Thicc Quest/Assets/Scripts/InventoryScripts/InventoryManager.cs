using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { set; get; }

    private Inventory inventory = new Inventory();

    public GameObject weaponSlot;
    public WeaponData activeWeapon;

	// Use this for initialization
	void Start ()
    {
        Instance = this;
        Load();
	}

    public void Load()
    {
        SaveLoadHanlder.Load(inventory, "/Inventory.dat", true);
    }

    public List<ItemData> GetNextItemList(int num, ItemType type, bool goBack = false)
    {
        return inventory.GetItemDataOfType(num, type, goBack);
    }

    public void AddItem(ItemData i)
    {
        inventory.AddItem(i);
        SaveLoadHanlder.Save(inventory, "/Inventory.dat", true);
    }

    public bool DiscardItem(ItemData id)
    {
        bool b =  inventory.RemoveItem(id);
        SaveLoadHanlder.Save(inventory, "/Inventory.dat", true);
        return b;
    }

    public bool ChangeWeapon(ItemData w)
    {
        if (w.type == ItemType.weapon)
        {
            activeWeapon = (WeaponData)w;
            weaponSlot.GetComponent<SpriteRenderer>().sprite = w.sprite;
            return true;
        }
        return false;
    }
}
