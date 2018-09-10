using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BodyScript : MonoBehaviour
{
    public List<BodySlot> slots;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public BodySlot GetSlot(string slotName)
    {
        foreach (BodySlot bs in slots)
        {
            if (bs.slotName == slotName) return bs;
        }
        return null;
    }

    public ItemData GetItemInSlot(string slotName)
    {
        BodySlot bs = GetSlot(slotName);
        if (bs == null) return null;
        else return bs.GetItem();
    }

    public bool ChangeGear(string slotName, ItemData item)
    {
        BodySlot slot = GetSlot(slotName);
        if (slot == null)
        {
            Debug.Log("Slot Name does not exsist");
            return false;
        }
        return slot.TryChange(item);
    }
}


