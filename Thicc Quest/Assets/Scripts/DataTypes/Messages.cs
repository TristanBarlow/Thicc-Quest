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
public class QuestionType : MessageType
{
    public Command y;
    public Command n;
    public QuestionType(int i, string m, Command yes, Command no ) : base(i, m)
    {
        y = yes;
        n = no;
    }
}
