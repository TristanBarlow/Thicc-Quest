using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public  class SaveLoadClass
{

    public const string PlayerMadeDir = "/PlayerMadeDir";
    public const string weaponsDir = "/Weapons/";
    public static String weaponPath = "";
    public const string InvetoryName = "/Inventory.dat";

    public static void SaveInventory(Inventory i)
    {
        string filePath = Application.persistentDataPath + "/Inventory.dat";

        FileStream file;
        //try and get the save game file
        if (File.Exists(filePath)) file = File.OpenWrite(filePath);
        else file = ResetFile(filePath);


        //serialise and save our data
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, i);
        file.Close();
    }

    public static Inventory LoadInventory()
    {
        string filePath = Application.persistentDataPath + "/Inventory.dat";
        FileStream file;
        Inventory i = new Inventory();
        //try and get save game file 
        if (File.Exists(filePath)) file = File.OpenRead(filePath);
        else
        {
            //if the file does not exsist create new local data class and save it;
            Debug.Log("Inventory Save not found, making one.");

            ResetFile(filePath);

            //exit load
            return i;
        }
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            i = (Inventory)bf.Deserialize(file);
            i.PrintInventory();
            file.Close();
            
        }
        catch(Exception ex)
        {
            file.Close();
            Debug.Log("Failed to read exsisting load file. Creating a new one" +ex );
            i = new Inventory();
            ResetFile(filePath);
        }
        return i;
    }

    public static void SaveWeapon(Texture2D texture, string name, WeaponData wd)
    {
        //Save the weapons image
        CreateFiles();
        string filePath = Application.persistentDataPath + PlayerMadeDir + weaponsDir + name;
        if (File.Exists(filePath + ".png")) File.Delete(filePath+".png"); ;

        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(filePath + ".png", bytes);


        //Save The weapons data
        FileStream file;
        //try and get the save game file
        if (File.Exists(filePath)) file = File.OpenWrite(filePath+".dat");
        else file = ResetFile(filePath+".dat");


        //serialise and save our data
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, wd);
        file.Close();
    }

    public static void LoadWeaponImages()
    {
        string dirPath = Application.persistentDataPath + PlayerMadeDir + weaponsDir;
        FileInfo[] files = TryGetFiles(dirPath, "*.png");
        if (files == null) return;

        foreach (FileInfo fi in files)
        {
            byte[] FileData = File.ReadAllBytes(dirPath + fi.Name);
            Texture2D Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
            if (Tex2D.LoadImage(FileData))
            {
                Sprite s = Sprite.Create(Tex2D, new Rect(0, 0, Tex2D.width, Tex2D.height), new Vector2(0.5f, 0.5f));
                string id = fi.Name.Remove(fi.Name.Length - 4);
                AssetManager.Instance.AddSpriteData( new SpriteData(id, s));
            }
        }
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
            AssetManager.Instance.AddSpriteData(new SpriteData(name, s));
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

    public static void LoadWeaponData()
    {
        string dirPath = Application.persistentDataPath + PlayerMadeDir + weaponsDir;
        FileInfo[] files = TryGetFiles(dirPath, "*.dat");
        if (files == null) return;
        foreach (FileInfo fi in files)
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                WeaponData wd  = (WeaponData)bf.Deserialize(fi.OpenRead());
                fi.OpenRead().Close();
                LootFactory.Instance.AddWeaponsToLoot(wd);
            }
            catch (Exception ex)
            {
                fi.OpenRead().Close();
                Debug.Log("Failed to read exsisting weapon data" + ex);
            }
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
