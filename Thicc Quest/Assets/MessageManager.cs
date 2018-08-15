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

    public  void NewQuestion(Command y, Command n, string message)
    {
        AddMessage(new QuestionType(1, message, y, n));
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
            case 1:
                QuestionMessage qm = new QuestionMessage(GetBoard(mt.id));
                qm.Apply(mt, speed);
                activeBoard = qm;
                break;
        }
    }

    public void TryQuestionReponse(bool r)
    {
        if (activeBoard.id == 1)
        {
            QuestionMessage qm = (QuestionMessage)activeBoard;
            qm.Response(r);
        }
        else { activeBoard.Dismissed = true; }
    }

    public void Dismiss()
    {

        if (activeBoard != null)
        {
            activeBoard.Dismissed = true;
        }
    }
}



