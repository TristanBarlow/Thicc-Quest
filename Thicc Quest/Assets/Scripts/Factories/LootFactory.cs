using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootFactory : MonoBehaviour
{

    public static LootFactory Instance { set; get; }

    private List<LootScript> l_Active = new List<LootScript>();
    private List<LootScript> l_Pool = new List<LootScript>();

    public int MaxNumberOfLoot = 30;

    public List<WeaponData> weapons;

    public GameObject lootPrefab;

    private void Start()
    {
        Instance = this;
        for (int i = 0; i < MaxNumberOfLoot; i++)
        {
            GameObject obj = Instantiate(lootPrefab, gameObject.transform);
            LootScript ls = obj.GetComponent<LootScript>();
            ls.Hide();
            l_Pool.Add(ls);
        }

        SaveLoadClass.LoadWeaponData();
    }

    public void SpawnLoot(Vector2 pos)
    {
        if (l_Pool.Count > 0)
        {
            LootScript ls = l_Pool[0];
            l_Pool.Remove(ls);
            l_Active.Add(ls);
            ItemData i = GetRandomWeaponData();
            i.sprite = AssetManager.Instance.GetSpriteFromId(i.spriteID);
            ls.Show(pos, i, this );
        }
    }

    public void SpawnLoot(Vector2 pos, ItemData i)
    {
        if (l_Pool.Count > 0)
        {
            LootScript ls = l_Pool[0];
            l_Pool.Remove(ls);
            l_Active.Add(ls);
            i.sprite = AssetManager.Instance.GetSpriteFromId(i.spriteID);
            ls.Show(pos, i, this);
        }
    }

    private WeaponData GetRandomWeaponData()
    {
        return weapons[Random.Range(0, weapons.Count)];
    }

    public void DespawnLoot(LootScript ls)
    {
        if (l_Active.Contains(ls))
        {
            l_Pool.Add(ls);
            l_Active.Remove(ls);
            GivePlayerItem(ls.data);
            ls.Hide();
        }
    }

    public void GivePlayerItem(ItemData i)
    {
        InventoryManager.Instance.AddItem(i);
    }

    public void AddWeaponsToLoot(WeaponData wd)
    {
        weapons.Add(wd);
    }
}
[System.Serializable]
public class SpriteData
{
    public Sprite sprite;
    public string ID;
    public SpriteData(string i, Sprite s)
    {
        ID = i;
        sprite = s;
    }
}