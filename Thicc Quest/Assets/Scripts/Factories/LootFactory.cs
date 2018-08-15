using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootFactory : MonoBehaviour
{

    public static LootFactory Instance { set; get; }

    private List<LootScript> l_Active = new List<LootScript>();
    private List<LootScript> l_Pool = new List<LootScript>();

    public int MaxNumberOfLoot = 30;

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

    }

    public void SpawnLoot(Vector2 pos)
    {
        if (l_Pool.Count > 0)
        {
            ItemData i = GetRandomWeaponData();
            GetLootPref().Show(pos, i, this );
        }
    }

    public void SpawnLoot(Vector2 pos, ItemData i)
    {
        if (l_Pool.Count > 0)
        {
            GetLootPref().Show(pos, i, this);
        }
    }

    private LootScript GetLootPref()
    {
        LootScript ls = l_Pool[0];
        l_Pool.Remove(ls);
        l_Active.Add(ls);
        return ls;
    }

    public void SpawnLootAroundPoint(Vector2 pos, ItemData id, int dist)
    {
        if (l_Pool.Count > 0)
        {
            Vector2 dir = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
            GetLootPref().Show(pos + (dir * dist), id, this);
        }
    }


    private WeaponData GetRandomWeaponData()
    {
        return WeaponManager.Instance.GetRandomWeapon();
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