using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FactoryManager : MonoBehaviour
{
    public TerrainFactory t_Factory;
    public int c_Unique = 5;
    public int c_Width = 10;
    public int c_Height = 10;

    public WorldObjectFactory w_Factory;
    public int maxPerChunk = 5;
    public int minPerChunk = 2;
    public int maxObj = 30;

    public bool load = false;

    private bool loaded = false;

    public GameObject player;
    private CharacterController_2D p_Script;

    public List<Vector2> playerNodes;


    public Dictionary<int, Vector2> ActiveSeeds =  new Dictionary<int, Vector2>();

    private float s_size;
    public float threshold =1.5f;

	// Use this for initialization
	void Start () {
		
	}
    
    public void InitGrasslandChunks()
    {
        t_Factory.Init(AssetManager.Instance.GetTerrainSprites(), AssetManager.Instance.GetWallSprites(), c_Unique, c_Width, c_Height);
        w_Factory.Init(minPerChunk, maxPerChunk, maxObj);
        loaded = true;
    }

	// Update is called once per frame
	void Update ()
    {
        if (load && !loaded)
        {
            InitGrasslandChunks();
           
            p_Script = player.GetComponent<CharacterController_2D>();
            playerNodes = p_Script.GetNodePos();

            s_size = AssetManager.Instance.GetSpriteSize();
        }
        if (loaded)
        {
            playerNodes = p_Script.GetNodePos();
            playerNodes.Add(player.transform.position);
            CheckForNewSeeds(playerNodes);
            CheckForOldSeeds(player.transform.position);
        }


    }


    public void  CheckForNewSeeds(List<Vector2> playerNodes)
    {
        foreach (Vector2 v2 in playerNodes)
        {
            Vector2 roundedPos = new Vector2(RoundOff(v2.x, c_Width * s_size), RoundOff(v2.y, c_Height * s_size));

            int seed = GetSeed(roundedPos);

            if (seed != -1)
            {
                ActiveSeeds.Add(seed, roundedPos);

                //New seed found Do all loading factory call here
                ChunkScript chunk = t_Factory.ActivateRandomChunk(roundedPos, seed);

                w_Factory.AddWorldObjectsToChunk(seed, chunk);
            }
        }
    }

    public void CheckForOldSeeds(Vector2 playerPos)
    {
        for (int i = 0; i < ActiveSeeds.Count; i++)
        {
            Vector2 center = ActiveSeeds.ElementAt(i).Value;
            if (playerPos.x < center.x + (c_Width * threshold) && playerPos.x > center.x - (c_Width * threshold) && playerPos.y < center.y + (c_Height * threshold) && playerPos.y > center.y - (c_Height * threshold))
            {

            }
            else
            {
                int key = ActiveSeeds.ElementAt(i).Key;
                //Seed is out of bounds, do all destroying here.
                t_Factory.DeactivateChunk(key);

                w_Factory.RemoveObjectsFromChunk(key);

                ActiveSeeds.Remove(key);
            }
        }
    }

    private int GetSeed(Vector2 pos)
    {
        string seed = pos.x.ToString() + pos.y.ToString();

        int newSeed = seed.GetHashCode();

        //If the two seeds are equal (the player has not moved out of this zone) stop generating
        if (ActiveSeeds.ContainsKey(newSeed))
        {
            return -1;
        }
        else
        {
            return newSeed;
        }
    }

    public static float RoundOff(float number, float round)
    {
        return (float)(round * System.Math.Round(number / round));
    }
}
