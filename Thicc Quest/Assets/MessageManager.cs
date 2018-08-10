using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour
{
    public static MessageManager Instance { set; get; }

    public List<MessageBoard> boards;

    public Transform startPos;
    public Transform endPos;

    private Queue<MessageType> messages = new Queue<MessageType>();

    public float speed = 5;

    private MessageBoard activeBoard;

    private float alpha= 0;

	// Use this for initialization
	void Start () {
        Instance = this;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (activeBoard!= null && activeBoard.board.activeSelf)
        {
            activeBoard.move(startPos.position, endPos.position);
            if (!activeBoard.board.activeSelf) CheckForNext();
        }
	}

    private MessageBoard GetBoard(int id)
    {
        foreach(MessageBoard mb in boards)
        {
            if (mb.id == id) return mb;
        }
        return null;
    }

    public void NewMessage(string m)
    {
       AddMessage(new MessageType(0, m));
    }

    private void AddMessage(MessageType mt)
    {
        if (activeBoard == null || !activeBoard.board.activeSelf)
        {
            ApplyMessage(mt);
        }
        else
        {
            messages.Enqueue(mt);
        }
    }

    public void CheckForNext()
    {
        if (messages.Count > 0)
        {
            ApplyMessage(messages.Dequeue());
        }
    }

    private void ApplyMessage(MessageType mt)
    {
        switch (mt.id)
        {
            case 0:
                activeBoard = GetBoard(mt.id);
                activeBoard.Apply(mt, speed);
                break;
        }
    }

    public void Dismiss()
    {
        if (activeBoard != null)
        {
            activeBoard.Dismissed = true;
        }
    }
}

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

    public void Apply(MessageType mt, float speed)
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

