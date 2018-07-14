using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class WeaponRecognition : MonoBehaviour
{
    public static WeaponRecognition Instance { set; get;  }

    public const string oneHanderLabel = "1HSword";

    Dictionary<string, WeaponReconSave> weaponDict = new Dictionary<string, WeaponReconSave>();

    public float accuracy = 0.05f;

    public float highAlphaCutoff = 0.9f;

    private void Start()
    {
        Instance = this;
    }
    private void Awake()
    {
        string savePath = Application.persistentDataPath;
        WeaponReconSave oneH = new WeaponReconSave(savePath, oneHanderLabel);
        weaponDict.Add(oneHanderLabel, oneH);

    }

    public void Train(Texture2D tex, string label)
    {
        Color[] arr = tex.GetPixels();
        int counter = 0;
        int HighAlpha = 0;
        Debug.Log("started Scan");
        WeaponReconSave wrs = weaponDict[label];
        wrs.ClearWeights();
        foreach (Color c in arr)
        {
            wrs.AddWeight(c.a);
            if (c.a > highAlphaCutoff) HighAlpha++;
            counter++;
        }
        wrs.highAlphaBound = HighAlpha;
        wrs.Save();
        Debug.Log("Finished Scan");
        Debug.Log("High alpha" + HighAlpha);
        Debug.Log(counter);
    }

    public bool ScanTexture(Texture2D tex, string label)
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
        if (Mathf.Abs(weaponDict[label].Compare(w_Alphas)) < weaponDict[label].highAlphaBound * accuracy)
        {
            Debug.Log("Match");
            return true;
        }
        else { Debug.Log("Not match"); }
        Debug.Log("High alpha" + HighAlpha);
        Debug.Log("Orignal Alpha high " + weaponDict[label].highAlphaBound);
        Debug.Log("Finished Scan");
        Debug.Log("Pixels: " + counter);
        return false;
    }
}


[System.Serializable]
public class WeaponReconSave
{
    string weaponLabel = "1HSword";
    string SavePath;
    List<float> indexWeightings;
    public int highAlphaBound = 0;

    public WeaponReconSave(string path, string label)
    {
        weaponLabel = label;
        SavePath = path + "/" + label + ".dat";
        TryLoad();
    }

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
        FileStream file;
        if (File.Exists(SavePath)) file = File.OpenRead(SavePath);
        else
        {
            NewInit();
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        WeaponReconSave wrs = (WeaponReconSave)bf.Deserialize(file);
        file.Close();
        OldInit(wrs);
        Debug.Log(indexWeightings.Count);
        Debug.Log("Loaded file from: " + SavePath);
    }

    private void NewInit()
    {
        indexWeightings = new List<float>();
    }

    private void OldInit(WeaponReconSave wrs)
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
