using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : InteractParent
{
    bool IsOpened = false;

    private ChestData chestData;

    private SpriteRenderer c_Sprite;

    public int MaxItemDrop = 4;

    public int SpawnRadius = 3;

    public void TryOpen()
    {
        if (!IsOpened)
        {
            IsOpened = true;
            c_Sprite.sprite = chestData.c_Opened;

            Vector2 mPos = transform.position;
            for(int i =0; i < Random.Range(1,MaxItemDrop);i++)
            {
                float rx = Random.Range(-SpawnRadius, SpawnRadius);
                float ry = Random.Range(-SpawnRadius, SpawnRadius);
                Vector2 v2 = new Vector2(rx + mPos.x, ry + mPos.y);
                LootFactory.Instance.SpawnLoot(v2);
            }
            
        }
    }

    public void Init(Vector2 pos)
    {
        chestData = AssetManager.Instance.GetRandomChest();
        gameObject.SetActive(true);
        gameObject.transform.position = pos;
        c_Sprite.sprite = chestData.c_Closed;
        IsOpened = false;
    }

	// Use this for initialization
	void Start ()
    {
        c_Sprite = GetComponent<SpriteRenderer>();
	}

    public void SetSpriteRenderer(SpriteRenderer sr)
    {
        c_Sprite = sr;
    }

    public void Hide()
    {
        IsOpened = false;
        gameObject.SetActive(false);
    }

    public override void Interact()
    {
        base.Interact();
        TryOpen();
    }

}
