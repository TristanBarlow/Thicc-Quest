using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WeaponMaker : MonoBehaviour {


    public Sprite baseSprite;

    private string SaveLocation = "/Resources/Sprites/PlayerMade/";

    // Use this for initialization
    void Start ()
    {
        SaveImage("Test");	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SaveImage(string name)
    {
        Texture2D texture =  baseSprite.texture;
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                Color color = Color.white;
                texture.SetPixel(x, y, color);
            }
        }
        byte[] bytes = baseSprite.texture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + SaveLocation+name+".png", bytes);
    }
}
