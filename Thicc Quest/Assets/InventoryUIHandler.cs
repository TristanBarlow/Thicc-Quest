using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIHandler : MonoBehaviour
{
    public static InventoryUIHandler Instance { set; get; }
    public Sprite slotBackImage;
    public Image itemDisplayImage;
    public Text itemNameText, itemDescriptionText;
    public List<InventorySlot> slots;


    private ItemType currentFilter = ItemType.all;

    public void RefreshSlots(bool goBack = false)
    {
        int i = 0;
        foreach (ItemData id in InventoryManager.Instance.GetNextItemList(slots.Count, currentFilter, goBack))
        {
            slots[i].Init(id);
            i++;
        }
        while (i < slots.Count)
        {
            slots[i].Reset(slotBackImage);
            i++;
        }
    }

    public void ToggleFilter(string newType)
    {
        switch (newType)
        {
            case "all":
                currentFilter = ItemType.all;
                break;
            case "armour":
                currentFilter = ItemType.armour;
                break;
            case "weapon":
                currentFilter = ItemType.weapon;
                break;
            case "item":
                currentFilter = ItemType.item;
                break;
        }
        RefreshSlots();
    }

    public void OnEnable()
    {
        RefreshSlots();
        Debug.Log("hi");
    }

    public void SlotClicked(int i)
    {
        ItemData id = slots[i].GetData();
        if (id != null)
        {
            itemNameText.text = id.name;
            itemDescriptionText.text = id.name + " is a " + id.type.ToString() + " nice.";
            itemDisplayImage.sprite = id.sprite;
        }
    }

    // Use this for initialization
    void Start () {
        Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
