using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    bool IsOpened = false;

    private ChestData chestData;

    private SpriteRenderer c_Sprite;

    public void TryOpen()
    {
        if (!IsOpened)
        {
            IsOpened = true;
            c_Sprite.sprite = chestData.c_Opened;

            //TO DO: Give item
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

}
