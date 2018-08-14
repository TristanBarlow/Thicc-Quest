using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public delegate bool ItemAction(ItemData id);
public enum ActionType
{
    Discard, Equip, Use, None
}

[System.Serializable]
public class AffinityStat
{
    public Text affText;
    public Text valueText;

    public void Init(Affinity af)
    {
        BaseAffinityData afd = AssetManager.Instance.GetBaseAff(af.type);
        affText.text = afd.title+ ": ";
        valueText.text = Math.Round(af.value, 2).ToString() + "%";
        
        affText.color = afd.col;
        valueText.color = afd.col;
    }
}
[System.Serializable]
public class UIBaseStat
{
    public Text titleText;
    public Text valueText;
    public GameObject obj;

    public void Init(string stat, int value)
    {
        obj.SetActive(true);
        titleText.text = stat;
        valueText.text = value.ToString();
    }
    public void Hide()
    {
        if (obj != null)
        {
            obj.SetActive(false);
        }
    }
}
[System.Serializable]
public class ItemActionButton
{
    public GameObject obj;
    public Text tex;
    private ItemAction action;
    private ItemData data;
    private ActionType type = ActionType.None;

    public void Init(string title, ItemAction ia, ItemData id, ActionType t)
    {
        obj.SetActive(true);
        tex.text = title;
        data = id;
        action = ia;
        type = t;
    }
    public void Hide()
    {
        if (obj != null)
        {
            obj.SetActive(false);
        }
    }
    public ActionType Use()
    {
        if (action != null)
        {
            if (action(data))
            {
                return type;
            }
            
        }
        return ActionType.None;
    }
}
