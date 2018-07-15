using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootScript : InteractParent
{
    public ItemData data;
    LootFactory factory;
    public override void Interact()
    {
        //To do implement add to inventory
        factory.DespawnLoot(this);
        base.Interact();
    }

    public void Show(Vector2 pos, ItemData i, LootFactory lf)
    {
        factory = lf;
        gameObject.SetActive(true);
        data = i;
        gameObject.transform.position = pos;
        gameObject.GetComponent<SpriteRenderer>().sprite = i.sprite;

    }
    public void Hide()
    {
        data = null;
        gameObject.SetActive(false);
    }

}
