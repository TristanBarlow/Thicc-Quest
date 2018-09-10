using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class PixelScanner
{
    public static AffinityData ScanTextureAffinity(Texture2D tex, BaseAffinityData[] affinities, float tolerance)
    {
        AffinityData affs = new AffinityData();

        Dictionary<AffType, int> values = new Dictionary<AffType, int>();
        
        //Add each affinity to the runnign values
        foreach (BaseAffinityData bad in affinities)
        {
            values.Add(bad.type, 0);
        }


        int numberOfActivePixels = 0;

        //Loop through and compare each pixel to the bas affinity color
        foreach (Color c in tex.GetPixels())
        {
            if (c.a > 0)
            {


                foreach (BaseAffinityData bad in affinities)
                {

                    if (bad.IsColorSimilar(tolerance, c))
                    {
                        values[bad.type]++;
                        numberOfActivePixels++;
                        break;
                    }

                }
            }
        }

        Debug.Log(numberOfActivePixels);
        //finally go through and set v;alues
        foreach (KeyValuePair<AffType, int> v in values)
        {
            affs.SetAffinityValue(v.Key, (float)v.Value / (float)numberOfActivePixels);
        }

        return affs;
    }
}

