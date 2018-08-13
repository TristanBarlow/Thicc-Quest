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
        SaveLoadClass.Load(inventory, "/Inventory.dat", true);
	}


    public List<ItemData> GetNextItemList(int num, ItemType type, bool goBack = false)
    {
        return inventory.GetItemDataOfType(num, type, goBack);
    }

    public void AddItem(ItemData i)
    {
        inventory.AddItem(i);
        SaveLoadClass.Save(inventory, "/Inventory.dat", true);
    }

    public void ChangeWeapon(WeaponData w)
    {
        activeWeapon = w;
        weaponSlot.GetComponent<SpriteRenderer>().sprite = w.sprite;
    }
}
