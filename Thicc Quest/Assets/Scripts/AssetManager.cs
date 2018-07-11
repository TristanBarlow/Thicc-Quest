using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour {
    

    public static AssetManager Instance { set; get; }

    //current sprite sheet to use t = terrain. w = wall
    private Sprite[] t_Sprites;
    private Sprite[] w_Sprites;
    private float s_Size;

    public void InitGrassLand()
    {
        t_Sprites = Resources.LoadAll<Sprite>("Sprites/Environment/Grassland");
        w_Sprites = Resources.LoadAll<Sprite>("Sprites/Environment/forest");
        s_Size = t_Sprites[0].bounds.size.x;
    }

    public Sprite[] GetWallSprites() { return w_Sprites; }
    public Sprite[] GetTerrainSprites() { return t_Sprites; }
    public float GetSpriteSize() { return s_Size; }

    // Use this for initialization
    void Start ()
    {
        Instance = this;
        InitGrassLand();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
