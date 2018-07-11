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

    public void DeactivateChunk(int seed)
    {
        if (!c_Active.ContainsKey(seed)) return;
        ChunkScript cs = c_Active[seed];
        c_Pool.Add(cs);
        cs.Hide();
        c_Active.Remove(seed);
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

    public void ActivateRandomChunk(Vector2 playerPos, int seed)
    {
        ChunkScript cs = c_Pool[Random.Range(0, c_Pool.Count)];
        cs.Use(playerPos);
        c_Pool.Remove(cs);
        c_Active.Add(seed, cs);
    }

}
