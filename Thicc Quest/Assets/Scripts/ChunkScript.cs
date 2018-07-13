using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkScript : MonoBehaviour
{

    List<GameObject> tiles = new List<GameObject>();

    public GameObject t_Prefab;
    public GameObject w_Prefab;

    private float s_Size;
    private float c_Width = 0;
    private float c_Height = 0;

    bool IsInitialised = false;

    private void Init(Sprite[]ss, float w, float h)
    {
        s_Size = ss[0].bounds.size.x;
        c_Width = w;
        c_Height = h;
        tiles = new List<GameObject>();
    }

    public void InitTerrain(Sprite[] t_Sprites, float width, float height)
    {
        Init(t_Sprites, width, height);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Sprite s = GetRandomSprite(t_Sprites);
                SpawnTile(s, t_Prefab, i, j, width, height);
            }
        }
    }

    public void InitWalledTerrain(Sprite[] t_Sprites, Sprite[] w_Sprites, float width, float height)
    {
        Init(w_Sprites, width, height);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject pref;
                Sprite s;
                if (Random.Range(0, 100) < 95)
                {
                    s = GetRandomSprite(t_Sprites);
                    pref = t_Prefab;
                }
                else
                {
                    s = GetRandomSprite(w_Sprites);
                    pref = w_Prefab;
                }
                SpawnTile(s, pref, i, j, width, height);
            }
        }
    }

    private void SpawnTile(Sprite s, GameObject def, int i, int j, float width, float height)
    {
        float x = (transform.position.x - width / 2) + (i * s.bounds.size.x);
        float y = (transform.position.y - height / 2) + (j * s.bounds.size.y);
        GameObject obj = Instantiate(def, new Vector2(x, y), transform.rotation, transform);
        obj.transform.SetParent(transform);
        obj.GetComponent<SpriteRenderer>().sprite = s;
        tiles.Add(obj);
    }

    public void Use(Vector2 newLocation)
    {
        transform.position = newLocation;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public Vector2 GetMaxBounds(float threshold)
    {
        return new Vector2(transform.position.x + (c_Width / 2) * threshold, transform.position.y + (c_Height / 2) * threshold);
    }

    public Vector2 GetMinBounds(float threshold)
    {
        return new Vector2(transform.position.x - (c_Width / 2) * threshold, transform.position.y - (c_Height / 2) * threshold);
    }

    private static Sprite GetRandomSprite(Sprite[] arr)
    {
        return arr[Random.Range(0, arr.Length)];
    }

    public GameObject GetRandomTile()
    {
        return tiles[Random.Range(0, tiles.Count)];
    }


    public float GetSpriteSize()
    {
        return s_Size;
    }
}
