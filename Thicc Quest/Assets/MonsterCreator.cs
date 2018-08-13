using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MonsterCreator : MonoBehaviour
{
    public string CurrentName;
    public Image targetImage;
    public Sprite baseSprite;
    public float Exploration = 0.1f;
    public float trainSpeed = 1.0f;
    public int width, height;
    public float edgeThreshold = 0.2f; 
    public bool ShouldLoadSave = false;

    DetailedTextureProfile currentProfile;

    private string savePath;

    public void Awake()
    {
        savePath = Application.persistentDataPath + "/TextureProfiles";
        if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);
    }

    public void Analyse()
    {
        if (currentProfile == null || currentProfile.name != CurrentName)
        {
            DetailedTextureProfile mtp = new DetailedTextureProfile(savePath, CurrentName, width, height, ShouldLoadSave);
            currentProfile = mtp;
        }
        ScanCurrentTexture();
    }

    public void ScanCurrentTexture()
    {
        currentProfile.StartNewScan(baseSprite.texture, trainSpeed);
        currentProfile.ApplyWeightedTexture(targetImage, Exploration);
        if(ShouldLoadSave)SaveLoadClass.Save(currentProfile, currentProfile.SavePath, false);
    }
}

[System.Serializable]
public class DetailedTextureProfile : ISave
{

    public string name = "";
    public string SavePath = "";
    List<PixelWeight> weights = new List<PixelWeight>();
    int timesTrained = 0;
    float averageEdgynessOfImage = 0.0f;
    int width, height;

    public DetailedTextureProfile(string savePath, string label, int w, int h, bool shouldLoad = false)
    {
        //If load is successfull these variables will be overriden by the saved version
        name = label;
        width = w;
        height = h;
        SavePath = savePath + "/" + label + ".dat";

        if (shouldLoad) SaveLoadClass.Load(this, SavePath, false);
        else LoadFailed();
    }

    public void LoadFailed()
    {
        for (int i = 0; i < width * height; i++)
        {
            weights.Add(new PixelWeight());
        }
        Debug.Log("Failed to load: " + name);
    }

    public Color[] GetPixels(float exploration)
    {
        Color[] arr = new Color[weights.Count];
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = weights[i].GetPixelColor();
            if (weights[i].Edgyness >= exploration*averageEdgynessOfImage) arr[i] = Color.black;
        }
        return arr;
    }

    public void LoadSuccess(object obj)
    {
        DetailedTextureProfile dtp = (DetailedTextureProfile)obj;
        weights = dtp.weights;
        name = dtp.name;
        width = dtp.width;
        height = dtp.height;
    }

    public Texture2D RemoveAlpha(Texture2D tex)
    {
        int w = tex.width;
        int h = tex.height;

        int minX = 0, maxX = tex.width, minY = 0, maxY = tex.height;

        for (int j = 0; j < tex.height; j++)
        {
            for (int i = 0; i < tex.width; i++)
            {
                if (tex.GetPixel(i, j).a != 0)
                {
                    maxY = j;
                    break;
                }
            }
        }
        for (int j = tex.height; j > 0; j--)
        {
            for (int i = 0; i < tex.width; i++)
            {
                if (tex.GetPixel(i, j).a != 0)
                {
                    minY = j;
                    break;
                }
            }
        }
        for (int i = 0; i < tex.width; i++)
        {
            for (int j = 0; j < tex.height; j++)
            {
                if (tex.GetPixel(i, j).a != 0)
                {
                    maxX = i;
                    break;
                }
            }
        }
        for (int i = tex.width; i > 0; i--)
        {
            for (int j = 0; j < tex.height; j++)
            {
                if (tex.GetPixel(i, j).a != 0)
                {
                    minX = i;
                    break;
                }
            }
        }
        Color[] newPixels = new Color[(maxX - minX) * (maxY - minY)];
        int counter = 0;
        for (int j = minY; j < maxY; j++)
        {
            for (int i = minX; i < maxX; i++)
            {
                newPixels[counter] = tex.GetPixel(i, j);
                counter++;
            }
        }
        Debug.Log(counter);
        Debug.Log("X: " +minX + ": " + maxX);
        Debug.Log("Y: " +minY + ": " + maxY);
        Texture2D tex2D = new Texture2D((maxX - minX), (maxY- minY), TextureFormat.ARGB32, false);
        tex2D.SetPixels(newPixels);
        tex2D.Apply();
        return tex2D;
    }

    public void StartNewScan(Texture2D tex, float trainSpeed)
    {
        timesTrained++;
        float currentModifier = trainSpeed;
        float xInterp = (float)tex.width / (float)width;
        float yInterp = (float)tex.height / (float)height;

        Color[] pixels = tex.GetPixels();
        float totalEdgyness = 0.0f;
        int counter =  0;

        for (int i = 1; i < height-2; i++)
        {
            for (int j = 1; j < width-2; j++)
            {
                int x = Mathf.RoundToInt(xInterp * j);
                int y = Mathf.RoundToInt(yInterp * i);
                weights[counter].ApplyWeight(tex.GetPixel(x, y), currentModifier);

                float magX = 0, magY = 0;

                

                weights[counter].Edgyness = Mathf.Sqrt(magX + magY);
                averageEdgynessOfImage += weights[counter].Edgyness;
                counter++;
            }
        }
        averageEdgynessOfImage = totalEdgyness / counter;
    }

    public void ApplyWeightedTexture(Image i, float exploration = 0)
    {
        Texture2D tex2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
        tex2D.SetPixels(GetPixels(exploration));
        tex2D.Apply();
        Sprite spr = Sprite.Create(tex2D, new Rect(new Vector2(0,0), new Vector2(width, height)), Vector2.zero);
        i.sprite = spr;
    }

}
[System.Serializable]
public class PixelWeight
{
    Col col = new Col(1,1,1,0);
    public float Edgyness = 0.0f;

    public void ApplyWeight(Color color, float modifer)
    {
        Col c = col.Subtract(color);
        col.r += c.r * modifer;
        col.g += c.g * modifer;
        col.b += c.b * modifer;
        col.a += c.a * modifer;
    }

    public float GetEdgyness(Color r)
    {
        Col c = col.Subtract(r);
        Edgyness = c.Magnitude;
        return Edgyness;
    }

    public Color GeneratePixelColor(float exploration)
    {
        float x, y, z, w;
        x = RandColorValue(col.r, exploration);
        y = RandColorValue(col.g, exploration);
        z = RandColorValue(col.b, exploration);
        w = RandColorValue(col.a, exploration);      
        return new Color(x, y, z, w);
    }

    public Color GetPixelColor()
    {
        return col.GetColor();
    }

    private float RandColorValue(float x, float maxVar)
    {
        float v = x * maxVar;
        return Random.Range(x - v, x + v);
    }
}
[System.Serializable]
public class Col
{
    public float r = 1, g = 1, b = 1, a = 1;

    public Col(Color c)
    {
        r = c.r;
        g = c.g;
        b = c.b;
        a = c.a;
    }
    public Col(float x, float y, float z, float w)
    {
        r = x;
        g = y;
        b = z;
        a = w;
    }
    public Col(Vector4 vec)
    {
        r = vec.x;
        g = vec.y;
        b = vec.z;
        a = vec.w;
    }
    public Col Subtract(Color c)
    {
        return new Col(c.r - r, c.g - g, c.b - b, c.a - a);
    }
    public static Col Subtract(Color c, Col col)
    {
        return new Col(c.r - col.r, c.g - col.g, c.b - col.b, c.a - col.a);
    }
    public Color GetColor()
    {
        return new Color(r, g, b, a);
    }
    public float Magnitude
    {
        get
        {
            return Mathf.Sqrt((r * r)+ (g * g)+ (b * b)+ 10*(a * a));
        }
    }
    public float Intensity
    {
        get
        {
            return (r + g + b + a) / 4;
        }
    }
}

public static class Kernel
{
    public static float[][] x3 = new float[3][] {new float[3] {-1, 0, 1 },
                                                 new float[3] {-2, 0, 2 },
                                                 new float[3] { -1, 0, 1}};

    public static float[][]y3 = new float[3][]{new float[3] {-1, -2, -1}, 
                                               new float[3] {0,  0,  0},
                                               new float[3] {1,  2,  1}};
}

