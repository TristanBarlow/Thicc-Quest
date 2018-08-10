using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MessageType
{
    public int id = 0;
    public string message;

    public MessageType(int i, string m)
    {
        id = i;
        message = m;
    }
}
