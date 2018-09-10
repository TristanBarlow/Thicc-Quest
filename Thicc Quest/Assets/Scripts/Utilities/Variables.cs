using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Variables: MonoBehaviour 
{
    public static Variables Instance { set; get; } 
    public float PixelScannerThrehold = 0.05f;
     void Start()
    {
        Instance = this;
    }
}

