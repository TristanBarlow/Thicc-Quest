using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { set; get; }
    private UIPreset activeCanvas;
    public List<UIPreset> canvasDict = new List<UIPreset>();

    public void Start()
    {
        Instance = this;
        ResetCanvas();
    }

    public bool ShouldUpdatePlayer()
    {
        if (activeCanvas.id == "map") return true;
        return false;
    }

    public void ResetCanvas()
    {
        foreach (UIPreset ui in canvasDict)
        {
            ui.canvas.SetActive(false);
        }
        ChangeCanvas("map");
    }

    public bool ChangeCanvas(string id)
    {
        UIPreset ui = GetUI(id);
        if (ui == null)
        {
            Debug.Log("Tried to change to: " + id + ", Menu not found");
            return false;
        }

        if (activeCanvas != null)
        {
            if (activeCanvas.id == id)
            {
                ResetCanvas();
                return false;
            }
            activeCanvas.canvas.SetActive(false);
        }

        CharacterController_2D.Instance.SwitchControls(ui.controlScheme);
        ui.canvas.SetActive(true);
        activeCanvas = ui;
        return true;

    }


    private UIPreset GetUI(string id)
    {
        foreach (UIPreset ui in canvasDict)
        {
            if (ui.id == id) return ui;
        }
        return null;
    }
}

[System.Serializable]
public class UIPreset
{
    public string id;
    public GameObject canvas;
    public ControlScheme controlScheme;
}
