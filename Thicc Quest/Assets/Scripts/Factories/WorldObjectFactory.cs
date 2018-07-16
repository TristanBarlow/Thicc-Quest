using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObjectFactory : MonoBehaviour
{
    public GameObject chestPrefab;

    private int MaxObjPerChunk;
    private int MinObjPerChunk;

    private Dictionary<int,List<ChestScript>> c_Active = new Dictionary<int, List<ChestScript>>();
    private List<ChestScript> c_Pool = new List<ChestScript>();

    // Use this for initialization
    void Start ()
    {
		
	}

    public void Init(int minPer, int maxPer,int totalMax)
    {
        MinObjPerChunk = minPer;
        MaxObjPerChunk = maxPer;
        for (int i = 0; i < totalMax; i++)
        {
            GameObject obj = Instantiate(chestPrefab, gameObject.transform);
            ChestScript cs = obj.GetComponent<ChestScript>();
            cs.SetSpriteRenderer(obj.GetComponent<SpriteRenderer>());
            cs.Hide();
            c_Pool.Add(cs);
        }
    }

    public void AddWorldObjectsToChunk(int seed, ChunkScript chunk)
    {
        List<ChestScript> chunkChests = new List<ChestScript>();
        int numToSpawn = Random.Range(MinObjPerChunk, MaxObjPerChunk);
        if (c_Pool.Count < numToSpawn) numToSpawn = c_Pool.Count;
        for (int i = 0; i <numToSpawn; i++)
        {
            if (c_Pool.Count < 1) return;

            GameObject tile = chunk.GetRandomTile();

            ChestScript chest = c_Pool[0];

            chest.Init(tile.transform.position);

            chunkChests.Add(chest);

           

            c_Pool.Remove(chest);
        }
        c_Active.Add(seed, chunkChests);
    }

    public void RemoveObjectsFromChunk(int seed)
    {
        if (!c_Active.ContainsKey(seed)) return;

        foreach (ChestScript chest in c_Active[seed])
        {
            c_Pool.Add(chest);
            chest.Hide();
            
        }
        c_Active.Remove(seed);
    }
}
