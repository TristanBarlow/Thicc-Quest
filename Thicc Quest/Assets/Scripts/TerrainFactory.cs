using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainFactory : MonoBehaviour
{
    public static TerrainFactory Instance { set; get; }

    public GameObject chunk;

    private List<ChunkScript> c_Pool = new List<ChunkScript>();
    private Dictionary<int, ChunkScript> c_Active = new Dictionary<int, ChunkScript>();

    private float s_size;
    void Start()
    {
        Instance = this;
    }

    public void TryGetNextChunk(List<Vector2> playerNodes, Vector2 playerPos, float threshold, float c_Width, float c_Height)
    {
        foreach (Vector2 v2 in playerNodes)
        {
            Vector2 roundedPos = new Vector2(RoundOff(v2.x, c_Width * s_size), RoundOff(v2.y, c_Height * s_size));

            int seed = GetSeed(roundedPos);

            if (seed != -1)
            {
                ActivateRandomChunk(roundedPos, seed);
            }
        }

        for(int i =0; i < c_Active.Count; i++)
        {
            ChunkScript cs = c_Active.ElementAt(i).Value;
            Vector2 max = cs.GetMaxBounds(1.0f);
            Vector2 min = cs.GetMinBounds(1.0f);
            if (playerPos.x < max.x+ (c_Width * threshold) && playerPos.x > min.x-(c_Width* threshold) && playerPos.y < max.y+(c_Height* threshold) && playerPos.y > min.y- (c_Height * threshold))
            {

            }
            else { DeactivateChunk(cs, i); }
        }
        
    }

    private void DeactivateChunk(ChunkScript cs, int i)
    {
        int seed = c_Active.ElementAt(i).Key;
        Debug.Log(c_Active.Keys.Count);
        c_Active.Remove(seed);
        c_Pool.Add(cs);
        cs.Hide();
    }

    public void Init(Sprite[] t_Sprites, Sprite[] w_Sprites, int c_Number, float c_Width, float c_Height)
    {
        for (int i = 0; i < c_Number; i++)
        {
            GameObject obj = Instantiate(chunk);
            ChunkScript cs = obj.GetComponent<ChunkScript>();
            cs.InitWalledTerrain(t_Sprites, w_Sprites, c_Width, c_Height);
            cs.Hide();
            s_size = cs.GetSpriteSize();
            c_Pool.Add(cs);
        }
    }

    private void ActivateRandomChunk(Vector2 playerPos, int seed)
    {
        ChunkScript cs = c_Pool[Random.Range(0, c_Pool.Count)];
        cs.Use(playerPos);
        c_Pool.Remove(cs);
        c_Active.Add(seed, cs);
    }

    private int GetSeed(Vector2 pos)
    {
        string seed = pos.x.ToString() + pos.y.ToString();

        int newSeed = seed.GetHashCode();

        //If the two seeds are equal (the player has not moved out of this zone) stop generating
        if (c_Active.ContainsKey(newSeed))
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
