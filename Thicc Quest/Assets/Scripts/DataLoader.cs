using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DataLoader : MonoBehaviour
{
    public static DataLoader Instance { set; get; }
    public GameObject loadingScreen;
    public Text messageText;
    public Dictionary<int, string> StuffLoading = new Dictionary<int, string>();
    bool loadingStarted = false;
    int ticketIter = 0;

    private void Start()
    {
        Instance = this;
        loadingScreen.SetActive(true);
    }

    public int AddLoadingItem(string message)
    {
        if (!loadingStarted) loadingStarted = true;
        StuffLoading.Add(ticketIter, message);
        ticketIter++;
        return ticketIter - 1;
    }

    public void LoadingFinished(int ticket)
    {
        StuffLoading.Remove(ticket);
        if (StuffLoading.Count < 1 && loadingStarted)
        {
            loadingScreen.SetActive(false);
        }
    }


    private void UpdateText()
    {
        string message = "Loading: ";
        for (int i = 0; i < StuffLoading.Count - 1; i++)
        {
            message += StuffLoading.ElementAt(i)+ ", " ;
        }
        message += StuffLoading.ElementAt(StuffLoading.Count) + "....";
        messageText.text = message;
    }
    public void LoadCustomWeapons()
    {

    }
}
