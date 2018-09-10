using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[System.Serializable]
public class BodySlot
{
    public string slotName = "";
    public string itemID = "";
    public ItemType slotType;
    public GameObject slotObj;
    private SpriteRenderer SlotSprite;
    private ItemData item;
    public bool TryChange(ItemData id)
    {

        if (SlotSprite == null)
        {
            SlotSprite = slotObj.GetComponent<SpriteRenderer>();
        }

        if (slotType == item.type)
        {
            item = id;
            SlotSprite.sprite = id.sprite;
            if (SlotSprite.sprite == null)
            {
                slotObj.SetActive(false);
            }
            else
            {
                slotObj.SetActive(true);
            }
            return true;
        }
        return false;
    }
    public ItemData GetItem()
    {
        if (item == null)
        {

        }
        return item;
    }
}
