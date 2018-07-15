using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { set; get; }

    public Inventory inventory = new Inventory();

    public GameObject weaponSlot;
    public WeaponData activeWeapon;

	// Use this for initialization
	void Start ()
    {
        Instance = this;
        inventory = SaveLoadClass.LoadInventory();
	}


    public void AddItem(ItemData i)
    {
        if (i.type == ItemType.weapon) ChangeWeapon((WeaponData)i);
        inventory.AddItem(i);
        SaveLoadClass.SaveInventory(inventory);
    }

    public void ChangeWeapon(WeaponData w)
    {
        activeWeapon = w;
        Debug.Log("ItemCnaged");
        weaponSlot.GetComponent<SpriteRenderer>().sprite = w.sprite;
    }
}
