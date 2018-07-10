using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryManager : MonoBehaviour
{

    public int c_Unique = 5;
    public int c_Width = 10;
    public int c_Height = 10;

    public bool load = false;

    private bool loaded = false;

    public GameObject player;
    private CharacterController_2D p_Script;

    public List<Vector2> playerNodes;

    public TerrainFactory t_Facotry;

	// Use this for initialization
	void Start () {
		
	}
    
   

    public void InitGrasslandChunks()
    {
        t_Facotry.Init(AssetManager.Instance.GetTerrainSprites(), AssetManager.Instance.GetWallSprites(), c_Unique, c_Width, c_Height);
    }

	// Update is called once per frame
	void Update ()
    {
        if (load && !loaded)
        {
            InitGrasslandChunks();
            loaded = true;
            p_Script = player.GetComponent<CharacterController_2D>();
            playerNodes = p_Script.GetNodePos();
            
        }
        if (loaded)
        {
            playerNodes = p_Script.GetNodePos();
            playerNodes.Add(player.transform.position);
            t_Facotry.TryGetNextChunk(playerNodes, player.transform.position, 2f, c_Width, c_Height);
        }


    }
}
