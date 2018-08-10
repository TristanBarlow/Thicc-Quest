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

    private string savePath;

    public void Awake()
    {
        savePath = Application.persistentDataPath + "/TextureProfiles";
        if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);
    }
    public void Analyse()
    {
        TextureProfile tp = new TextureProfile(savePath, CurrentName);

    }
}

public class MonsterTextureProfile : ISave
{
    public void LoadFailed()
    {
        throw new System.NotImplementedException();
    }

    public void LoadSuccess(object obj)
    {
        throw new System.NotImplementedException();
    }
}
