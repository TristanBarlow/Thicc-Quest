using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour {
    

    public static AssetManager Instance { set; get; }

    ///-------------Ranks-------------\\\
    public Rank[] ranks;

    public Rank GetRank(float evaluation)
    {
        foreach (Rank r in ranks)
        {
            if (evaluation < r.req) return r;
        }
        return ranks[ranks.Length-1];
    }

    
    ///------------- Affinities ----------\\\\\
    public BaseAffinityData[] baseAffinities;
    public Color GetAffColor(AffType type)
    {
        foreach (BaseAffinityData aff in baseAffinities)
        {
            if (aff.type == type) return aff.col;
        }
        return Color.magenta;
    }
    public BaseAffinityData GetBaseAff(AffType type)
    {
        foreach (BaseAffinityData bad in baseAffinities)
        {
            if (bad.type == type)
            {
                return bad;
            }
        }
        return baseAffinities[0];
    }


    ///------------------ Environment stuff ---------\\\\\\\\\\\
    //current sprite sheet to use t = terrain. w = wall
    private Sprite[] t_Sprites;
    private Sprite[] w_Sprites;
    private float s_Size;
    public Sprite[] GetWallSprites() { return w_Sprites; }
    public Sprite[] GetTerrainSprites() { return t_Sprites; }
    public float GetSpriteSize() { return s_Size; }
    public void InitGrassLand()
    {
        t_Sprites = Resources.LoadAll<Sprite>("Sprites/Environment/Grassland");
        w_Sprites = Resources.LoadAll<Sprite>("Sprites/Environment/forest");
        s_Size = t_Sprites[0].bounds.size.x;
    }


    ///-------------- Chest stuff----------\\\\\
    public ChestData[] Chests;
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
    ///-------------- Ore stuff ---------\\\\
    public OreParent[] ores;
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
        SaveLoadHanlder.LoadWeaponImages();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

}
