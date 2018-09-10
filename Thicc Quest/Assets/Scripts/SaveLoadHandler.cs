using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoadHanlder
{

    public const string PlayerMadeDir = "/PlayerMadeDir";
    public const string weaponsDir = "/Weapons/";
    public static String weaponPath = "";
    public const string InvetoryName = "/Inventory.dat";

    public static void SaveWeapon(Texture2D texture, string name, WeaponData wd)
    {
        //Save the weapons image
        CreateFiles();
        string filePath = Application.persistentDataPath + PlayerMadeDir + weaponsDir + name;
        if (File.Exists(filePath + ".png")) File.Delete(filePath + ".png"); ;

        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(filePath + ".png", bytes);


        //Save The weapons data
        FileStream file;
        //try and get the save game file
        if (File.Exists(filePath)) file = File.OpenWrite(filePath + ".dat");
        else file = ResetFile(filePath + ".dat");


        //serialise and save our data
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, wd);
        file.Close();
    }

    public static void DeleteWeapon(string name)
    {
        //Save the weapons image
        string filePath = Application.persistentDataPath + PlayerMadeDir + weaponsDir + name;
        if (File.Exists(filePath + ".png")) File.Delete(filePath + ".png");

        //try and get the save game file
        if (File.Exists(filePath + ".dat"))File.Delete(filePath + ".dat");
    }

    public static List<SpriteData> LoadWeaponImages()
    {
        string dirPath = Application.persistentDataPath + PlayerMadeDir + weaponsDir;
        FileInfo[] files = TryGetFiles(dirPath, "*.png");

        List<SpriteData> sprites = new List<SpriteData>();

        if (files == null) return null;

        foreach (FileInfo fi in files)
        {
            byte[] FileData = File.ReadAllBytes(dirPath + fi.Name);
            Texture2D Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
            if (Tex2D.LoadImage(FileData))
            {
                Sprite s = Sprite.Create(Tex2D, new Rect(0, 0, Tex2D.width, Tex2D.height), new Vector2(0.5f, 0.5f));
                string id = fi.Name.Remove(fi.Name.Length - 4);
                sprites.Add(new SpriteData(id, s));
            }
        }
        return sprites;
    }

    public static void LoadSprite(string name)
    {
        string dirPath = Application.persistentDataPath + PlayerMadeDir + weaponsDir + name + ".png";
        if (!File.Exists(dirPath)) return;
        byte[] FileData = File.ReadAllBytes(dirPath);
        Texture2D Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
        if (Tex2D.LoadImage(FileData))
        {
            Sprite s = Sprite.Create(Tex2D, new Rect(0, 0, Tex2D.width, Tex2D.height), new Vector2(0.5f, 0.5f));
            ItemManager.Instance.AddSpriteData(new SpriteData(name, s));
        }
    }

    public static FileInfo[] TryGetFiles(string dir, string ext)
    {
        DirectoryInfo di = new DirectoryInfo(dir);
        FileInfo[] fi = null;
        try
        {
            return di.GetFiles(ext);
        }
        catch (Exception ex)
        {
            Debug.Log("No files found");
            return null;
        }

    }

    public static List<WeaponData> LoadWeaponData()
    {
        string dirPath = Application.persistentDataPath + PlayerMadeDir + weaponsDir;
        FileInfo[] files = TryGetFiles(dirPath, "*.dat");
        List<string> filesToGo = new List<string>();
        if (files == null) return null;

        FileStream fs = null;

        List<WeaponData> weapons = new List<WeaponData>();

        foreach (FileInfo fi in files)
        {
            try
            {
                fs = fi.OpenRead();
                BinaryFormatter bf = new BinaryFormatter();
                WeaponData wd = (WeaponData)bf.Deserialize(fs);
                fs.Close();
                weapons.Add(wd);
            }
            catch (Exception ex)
            {
                if (fs != null)
                {
                    fs.Close();
                }
                Debug.Log("Failed to read exsisting weapon data" + ex);
                Debug.Log("Deleting file : " + fi.Directory +"/" +fi.Name);
                filesToGo.Add(fi.Name);
            }
        }


        foreach (string n in filesToGo)
        {
            File.Delete(dirPath + n);
        }


        return weapons;
    }

    public static void Save<T>(T arg, string SavePath, bool needsDefaultPath = false)
    {
        string path= "";
        if (needsDefaultPath) path += Application.persistentDataPath + SavePath;
        else path = SavePath;

        FileStream file;
        if (File.Exists(path)) file = File.OpenWrite(path);
        else file = File.Create(path);


        //serialise and save our data
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, arg);
        file.Close();
    }

    public static void Load<T>(T arg, string SavePath, bool needsDefaultPath = false) where T :ISave
    {
        string path = "";
        if (needsDefaultPath) path += Application.persistentDataPath + SavePath;
        else path = SavePath;
        FileStream file = null;
        try
        {
            if (File.Exists(path)) file = File.OpenRead(path);
            else
            {
                arg.LoadFailed();
                return;
            }
            BinaryFormatter bf = new BinaryFormatter();
            T wrs = (T)bf.Deserialize(file);
            file.Close();
            arg.LoadSuccess(wrs);
            Debug.Log("Loaded file from: " + path);
        }
        catch(System.Exception ex)
        {
            if (file != null) file.Close();
            arg.LoadFailed();
            Debug.Log("Failed load path " + path + ex);
        }

    }

    public static FileStream ResetFile(string destination)
    {
        //delete one if already exsists
        if (File.Exists(destination)) File.Delete(destination);

        //create new file and data for use
        return File.Create(destination);

    }
    public static bool DoesFileExsist(string path)
    {
        if (File.Exists(path)) return true;
        return false;
    }
    public static void CreateFiles()
    {
        string path = Application.persistentDataPath; // your code goes here
        {
            bool exists = System.IO.Directory.Exists(path + PlayerMadeDir);
            if (!exists)
                System.IO.Directory.CreateDirectory(path + PlayerMadeDir);
        }
        {
            bool exists = System.IO.Directory.Exists(path + PlayerMadeDir+ weaponsDir);
            if (!exists)
                System.IO.Directory.CreateDirectory(path + PlayerMadeDir + weaponsDir);
        }
    }
}
