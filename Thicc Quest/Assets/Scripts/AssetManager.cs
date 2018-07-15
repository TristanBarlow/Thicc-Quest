using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour {
    

    public static AssetManager Instance { set; get; }

    //current sprite sheet to use t = terrain. w = wall
    private Sprite[] t_Sprites;
    private Sprite[] w_Sprites;
    private float s_Size;

    public OreParent[] ores;
    public ChestData[] Chests;

    public void InitGrassLand()
    {
        t_Sprites = Resources.LoadAll<Sprite>("Sprites/Environment/Grassland");
        w_Sprites = Resources.LoadAll<Sprite>("Sprites/Environment/forest");
        s_Size = t_Sprites[0].bounds.size.x;
    }

    public Sprite[] GetWallSprites() { return w_Sprites; }
    public Sprite[] GetTerrainSprites() { return t_Sprites; }

    public ChestData GetChestFromID(int ID)
    {
        foreach (ChestData c in Chests)
        {
            if (c.ID == ID) return c;
        }
        return null; 
    }

    public ChestData GetRandomChest()
    {
        return Chests[Random.Range(0, Chests.Length)];
    }

    public float GetSpriteSize() { return s_Size; }

    public OreParent GetOreOfType(OreType ore)
    {
        foreach (OreParent op in ores)
        {
            if (op.type == ore) return op;
        }
        return ores[0];
    }
   
    // Use this for initialization
    void Start ()
    {
        Instance = this;
        InitGrassLand();
        SaveLoadClass.LoadWeaponImages();
    }
	
	// Update is called once per frame
	void Update () {
		
	}



    public List<SpriteData> weaponSprites;
    public Sprite GetSpriteFromId(string id)
    {
        foreach(SpriteData sd in weaponSprites)
        {
            if (sd.ID == id) return sd.sprite;
        }
        return null;
    }
    public void AddSpriteData(SpriteData sd)
    {
        weaponSprites.Add(sd);
    }
}
