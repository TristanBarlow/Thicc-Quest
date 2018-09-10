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
        LootFactory.Instance.SpawnLootAroundPoint(CharacterController_2D.Instance.transform.position, id, 1);
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

    public bool DestroyAllOfItem(ItemData id)
    {
        bool b = true;
        while (b)
        {
            b = inventory.RemoveItemByID(id.name);
        }
        SaveLoadHanlder.DeleteWeapon(id.name);
        SaveLoadHanlder.Save(inventory, "/Inventory.dat", true);
        return true;
    }
}
