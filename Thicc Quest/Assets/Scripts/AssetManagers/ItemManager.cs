using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum WeaponSortType
{
    active, normal, playerMade
}

class ItemManager:MonoBehaviour 
{
    public static ItemManager Instance { set; get; }
    public List<WeaponData> baseWeapons = new List<WeaponData>();
    public List<ArmourData> baseArmours = new List<ArmourData>();
    public List<ItemData> baseItems = new List<ItemData>();

    private List<WeaponData> activeWeapons = new List<WeaponData>();
    private List<WeaponData> playerMadeWeapons = new List<WeaponData>();

    public List<SpriteData> Sprites = new List<SpriteData>();

    public bool AllWeaponsAreActive = true;

    private void Start()
    {
        Instance = this;
        AddToActiveWeapons(baseWeapons);
        LoadSequence();
    }

    public void LoadSequence()
    {
        LoadPlayerMadeData();
    }

    public void AddToActiveWeapons(List<WeaponData> wds)
    {
        foreach (WeaponData wd in wds)
        {
            activeWeapons.Add(wd);
        }
    }

    public void RemoveFromActiveWeapons(List<WeaponData> wds)
    {
        foreach (WeaponData wd in wds)
        {
            if (activeWeapons.Contains(wd)) activeWeapons.Remove(wd);
        }
    }
    public void LoadPlayerMadeData()
    {
        List<WeaponData> data = SaveLoadHanlder.LoadWeaponData();
        Sprites.AddRange(SaveLoadHanlder.LoadWeaponImages());

        foreach (WeaponData wd in data)
        {
            AddWeapon(wd, WeaponSortType.playerMade);
        }
        
     
    }


    public void AddWeaponSprite(SpriteData sd)
    {
        Sprites.Add(sd);
    }

    public Sprite GetSpriteFromId(string id)
    {
        foreach (SpriteData sd in Sprites)
        {
            if (sd.ID == id) return sd.sprite;
        }
        return null;
    }

    public void AddSpriteData(SpriteData sd)
    {
        Sprites.Add(sd);
    }

    public WeaponData GetWeaponFromName(string name)
    {
        foreach (WeaponData wd in baseWeapons)
        {
            if (wd.name == name) return wd;
        }
        foreach (WeaponData wd in playerMadeWeapons)
        {
            if (wd.name == name) return wd;
        }
        return null;
    }

    public void AddWeapon(WeaponData wd, WeaponSortType type)
    {
        switch (type)
        {
            case WeaponSortType.active:
                activeWeapons.Add(wd);
                break;
            case WeaponSortType.playerMade:
                playerMadeWeapons.Add(wd);
                break;
            case WeaponSortType.normal:
                baseWeapons.Add(wd);
                break;
                
        }
        if (type != WeaponSortType.active && AllWeaponsAreActive)
        {
            activeWeapons.Add(wd);
        }
    }

    public void RemoveWeaponFromLoot(string id)
    {
        for (int i = 0; i < activeWeapons.Count; i++)
        {
            if (activeWeapons[i].uID == id)
            {
                activeWeapons.Remove(activeWeapons[i]);
            }
        }
    }

    public void AddWeaponsToLoot(WeaponData wd)
    {
        activeWeapons.Add(wd);
    }

    public WeaponData GetRandomWeapon()
    {
        return activeWeapons[UnityEngine.Random.Range(0, activeWeapons.Count)];
    }
}
