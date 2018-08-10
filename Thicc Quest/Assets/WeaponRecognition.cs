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
        Debug.Log("started Scan");
        int HighAlpha = 0;
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
        Debug.Log("High alpha" + HighAlpha);
        Debug.Log("Orignal Alpha high " + baseWeap.highAlphaBound);
        Debug.Log("Finished Scan");
        Debug.Log("Pixels: " + counter);
        return false;
    }
}


[System.Serializable]
public class TextureProfile
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

    public void Save()
    {
        FileStream file;
        if (File.Exists(SavePath)) file = File.OpenWrite(SavePath);
        else file = File.Create(SavePath);


        //serialise and save our data
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, this);
        file.Close();
    }

    private void TryLoad()
    {
        FileStream file = null;
        try
        {
            if (File.Exists(SavePath)) file = File.OpenRead(SavePath);
            else
            {
                NewInit();
                return;
            }
            trained = true;
            BinaryFormatter bf = new BinaryFormatter();
            TextureProfile wrs = (TextureProfile)bf.Deserialize(file);
            file.Close();
            OldInit(wrs);
            Debug.Log(indexWeightings.Count);
            Debug.Log("Loaded file from: " + SavePath);
        }
        catch
        {
            if(file != null)  file.Close();
            Debug.Log("Failed load path " + SavePath);
            File.Delete(SavePath);
        }
    
    }

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
}
