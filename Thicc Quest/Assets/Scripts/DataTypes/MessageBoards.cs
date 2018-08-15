using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public delegate void Command();

[System.Serializable]
public class MessageBoard
{
    public int id = 0;
    public GameObject board;
    public Text t;
    public float alpha = 0;

    private bool dismiss = false;
    private float dir = 1;

    public bool Dismissed
    {
        set
        {
            if (value)
            {
                dir *= -1;
            }
            dismiss = value;
        }
        get
        {
            return dismiss;
        }
    }

    public virtual void Apply(MessageType mt, float speed)
    {
        t.text = mt.message;
        dir = 1;
        dismiss = false;
        dir *= speed;
        alpha = 0;
        board.SetActive(true);
    }
    public void move(Vector2 srt, Vector2 end)
    {
        if (alpha >= 1 && !dismiss) return;
        alpha += Time.deltaTime * dir;
        board.transform.position = Vector2.Lerp(srt, end, alpha);
        if (dismiss && alpha < 0) board.SetActive(false);
    }
}



public class QuestionMessage:MessageBoard
{
    Command noFunc;
    Command yeaFunc;

    public override void Apply(MessageType mt, float speed)
    {
        QuestionType qt = (QuestionType)mt;
        noFunc = qt.n;
        yeaFunc = qt.y;
        base.Apply(mt, speed);
    }

    public QuestionMessage(MessageBoard mb)
    {
        board = mb.board;
        t = mb.t;
        id = mb.id;
    }

    public void Response(bool respone)
    {
        if (respone)
        {
            if (yeaFunc != null)
            {
                yeaFunc();
            }
        }
        else
        {
            if (noFunc != null)
            {
                noFunc();
            }
        }
        Dismissed = true;
    }
}
