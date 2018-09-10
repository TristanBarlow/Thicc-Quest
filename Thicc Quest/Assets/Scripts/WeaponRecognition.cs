using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class WeaponRecognition : MonoBehaviour
{
    public static WeaponRecognition Instance { set; get;  }

    public const string oneHanderLabel = "1HSword";
    public static string savePath = "";

    Dictionary<string, TextureProfile> weaponDict = new Dictionary<string, TextureProfile>();

    public float accuracy = 0.05f;

    public float highAlphaCutoff = 0.9f;

    TextureProfile currentWeapon;

    private void Start()
    {
        Instance = this;
    }
    private void Awake()
    {
        savePath = Application.persistentDataPath + "/TextureProfiles";

        if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);
    }

    public void LoadWeapon(Sprite s, string weaponLabel)
    {
        TextureProfile weap = new TextureProfile(savePath, weaponLabel);
        if (!weap.Trained)
        {
            Train(s.texture, ref weap);
        }
        currentWeapon = weap;
    }

    public void Train(Texture2D tex, ref TextureProfile wrs)
    {
        Color[] arr = tex.GetPixels();
        int counter = 0;
        int HighAlpha = 0;
        Debug.Log("started Scan");
        wrs.ClearWeights();
        foreach (Color c in arr)
        {
            wrs.AddWeight(c.a);
            if (c.a > highAlphaCutoff) HighAlpha++;
            counter++;
        }
        wrs.highAlphaBound = HighAlpha;
        wrs.Trained = true;
        wrs.Save();
        Debug.Log("Finished Scan");
        Debug.Log("High alpha" + HighAlpha);
        Debug.Log(counter);
    }

    public bool CheckTexture(Texture2D tex)
    {
        if (currentWeapon == null) return false;
        else return ScanActiveTexture(tex, currentWeapon);
    }

    public bool ScanActiveTexture(Texture2D tex, TextureProfile baseWeap)
    {
        
        Color[] arr  = tex.GetPixels();
        int counter = 0; 
        int HighAlpha = 0;

        bool DEBUG = false;

        List<float> w_Alphas = new List<float>();
        foreach (Color c in arr)
        {
            w_Alphas.Add(c.a);
            if (c.a > highAlphaCutoff) HighAlpha++;
            counter++;
        }
        if (Mathf.Abs(baseWeap.Compare(w_Alphas)) < baseWeap.highAlphaBound * accuracy)
        {
            Debug.Log("Match");
            return true;
        }
        else { Debug.Log("Not match"); }
        if (DEBUG)
        {
            Debug.Log("High alpha" + HighAlpha);
            Debug.Log("Orignal Alpha high " + baseWeap.highAlphaBound);
            Debug.Log("Finished Scan");
            Debug.Log("Pixels: " + counter);
        }
        return false;
    }
}


[System.Serializable]
public class TextureProfile:ISave
{
    string weaponLabel = "1HSword";
    string SavePath;
    bool trained = false;

    public int TrainedSize = 0;

    List<float> indexWeightings;

    public int highAlphaBound = 0;

    public TextureProfile(string path, string label)
    {
        weaponLabel = label;
        SavePath = path + "/" + label + ".dat";
        TryLoad();
    }

    public bool Trained { set { trained = value; } get { return trained; } }

  
    private void NewInit()
    {
        indexWeightings = new List<float>();
    }

    private void OldInit(TextureProfile wrs)
    {
        indexWeightings = wrs.indexWeightings;
        weaponLabel = wrs.weaponLabel;
        highAlphaBound = wrs.highAlphaBound;

    }

    public void AddWeight(float w)
    {
        if (indexWeightings != null) indexWeightings.Add(w);
        else { Debug.Log("Err weighitngs = null"); }
    }

    public void ClearWeights()
    {
        indexWeightings.Clear();
    }

    public float Compare(List<float> weights)
    {
        if (weights.Count != indexWeightings.Count)
        {
            Debug.Log("Pixel sizes do not match Compare Failed");
            //return 100000.0f;
        }
        Debug.Log("Started Compare");
        float differenceTotal = 0.0f;
        int i;
        for (i=0;  i < weights.Count && i < indexWeightings.Count;i++)
        {
            differenceTotal += indexWeightings[i] - weights[i];
        }
        while(i < weights.Count)
        {
            differenceTotal -= 1.0f;
            i++;
        }
        while (i < indexWeightings.Count)
        {
            differenceTotal -= 1.0f;
            i++;
        }
        Debug.Log("Finished compare. Difference = " + differenceTotal);
        return differenceTotal;
    }

    public void Save()
    {
        SaveLoadHanlder.Save(this, SavePath);
    }

    public void TryLoad()
    {
        SaveLoadHanlder.Load(this, SavePath);
    }
    public void LoadSuccess(object obj)
    {
        TextureProfile wrs = (TextureProfile)obj;
        OldInit(wrs);
    }

    public void LoadFailed()
    {
        Debug.Log("Failed load");
        NewInit();
    }
}
