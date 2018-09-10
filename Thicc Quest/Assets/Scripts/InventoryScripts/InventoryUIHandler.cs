using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIHandler : MonoBehaviour
{
    public static InventoryUIHandler Instance { set; get; }
    public Sprite slotBackImage;
    public Image itemDisplayImage;
    public Text itemNameText, itemDescriptionText;
    public List<InventorySlot> slots;

    private ItemData activeItem;
    private ItemData ActiveItem
    {set
        {
            activeItem = value;
            RefreshNameDescrText();
            if (IsShowing)
            {
                RefreshStats();
            }
        }
        get { return activeItem; }
}

    public float eval = 0.0f;
    public GameObject statsLayer;
    public GameObject affinityLayer;
    public List<AffinityStat> affStats;
    public List<UIBaseStat> baseStats;
    public List<ItemActionButton> actionButtons;
    public Text MoreButton, rank;
    private bool IsShowing = false;

    private ItemType currentFilter = ItemType.all;

    public void RefreshSlots(bool goBack = false)
    {
        int i = 0;
        if (InventoryManager.Instance == null) return;
        foreach (ItemData id in InventoryManager.Instance.GetNextItemList(slots.Count, currentFilter, goBack))
        {
            slots[i].Init(id);
            i++;
        }
        while (i < slots.Count)
        {
            slots[i].Reset(slotBackImage);
            i++;
        }
        ActiveItem = slots[0].GetData();
        CheckHasItems();
    }

    public void ToggleFilter(string newType)
    {
        switch (newType)
        {
            case "all":
                currentFilter = ItemType.all;
                break;
            case "armour":
                currentFilter = ItemType.armour;
                break;
            case "weapon":
                currentFilter = ItemType.weapon;
                break;
            case "item":
                currentFilter = ItemType.item;
                break;
        }
        RefreshSlots();

    }

    public void OnEnable()
    {
        RefreshSlots();
    }

    public void RefreshNameDescrText()
    {
        if (ActiveItem == null)
        {
            itemNameText.text = "";
            itemDescriptionText.text = "";
            itemDisplayImage.sprite = slotBackImage;
        }
        else
        {
            itemNameText.text = ActiveItem.name;
            itemDescriptionText.text = ActiveItem.name + " is a " + ActiveItem.type.ToString() + ", nice.";
            itemDisplayImage.sprite = ActiveItem.sprite;
        }
    }

    public void CheckHasItems()
    {
        if (slots[0].GetData() == null)
        {
            MessageManager.Instance.NewMessage("You have no " + currentFilter.ToString() +"s, try getting some.");
            HideStats();
        }
    }

    public void SlotClicked(int i)
    {
        if (i > slots.Count - 1) return;
        ItemData id = slots[i].GetData();
        if (id != null)
        {
            ActiveItem = id;
        }
        if (IsShowing)
        {
            RefreshStats();
        }
    }

    public void RefreshStats()
    {
        bool hasAffin = false;
        if (activeItem == null)
        {
            ToggleMore();
            return;
        }
        switch (ActiveItem.type)
        {
            case ItemType.armour:
                {
                    hasAffin = true;
                    StatsArmourInit((ArmourData)ActiveItem);
                    break;
                }
            case ItemType.item:
                {
                    StatsItemsInit(ActiveItem);
                    break;
                }
            case ItemType.weapon:
                {
                    StatsWeaponInit((WeaponData)ActiveItem);
                    hasAffin = true;
                    break;
                }
        }
        if (hasAffin)
        {
            affinityLayer.SetActive(true);
        }
        else affinityLayer.SetActive(false);

        Rank r = AssetManager.Instance.GetRank(activeItem.Evaluate());
        rank.text = r.title;
        rank.color = r.col;
    }

    private void StatsArmourInit(ArmourData id)
    {
        baseStats[0].Init(id.resistance.Title, id.resistance.value);
        baseStats[1].Init(id.magicResist.Title, id.magicResist.value);
        baseStats[2].Init(id.weight.Title, id.weight.value);
        baseStats[3].Init(id.quality.Title, id.quality.value);
        ApplyAffinityData(id.affinities.GetAffinities());

        actionButtons[0].Init("Equip", InventoryManager.Instance.ChangeWeapon,ActiveItem, ActionType.Equip);
        actionButtons[1].Init("Discard", InventoryManager.Instance.DiscardItem,ActiveItem, ActionType.Discard);
        actionButtons[2].Hide();
        actionButtons[3].Hide();
    }
    private void StatsWeaponInit(WeaponData id)
    {
        baseStats[0].Init(id.damage.Title, id.damage.value);
        baseStats[1].Init(id.speed.Title, id.speed.value);
        baseStats[2].Init(id.weight.Title, id.weight.value);
        baseStats[3].Init(id.quality.Title, id.quality.value);
        ApplyAffinityData(id.affinities.GetAffinities());
        actionButtons[0].Init("Equip", InventoryManager.Instance.ChangeWeapon, ActiveItem, ActionType.Equip);
        actionButtons[1].Init("Discard", InventoryManager.Instance.DiscardItem, ActiveItem, ActionType.Discard);
        actionButtons[2].Init("Destroy All", InventoryManager.Instance.DestroyAllOfItem, ActiveItem, ActionType.Discard);
        actionButtons[3].Hide();
    }
    private void StatsItemsInit(ItemData id)
    {
        baseStats[0].Init(id.weight.Title, id.weight.value);
        baseStats[1].Init(id.quality.Title, id.quality.value);
        baseStats[2].Hide();
        baseStats[3].Hide();

        actionButtons[0].Init("Use", InventoryManager.Instance.ChangeWeapon, ActiveItem, ActionType.Use);
        actionButtons[1].Init("Discard", InventoryManager.Instance.DiscardItem, ActiveItem, ActionType.Discard);
        actionButtons[2].Hide();
        actionButtons[3].Hide();
    }


    public void ItemAction(int i)
    {
       ActionType type = actionButtons[i].Use();
        switch (type)
        {
            case ActionType.Discard:
                MessageManager.Instance.NewMessage("You have discarded your " + activeItem.name );
                RefreshSlots();
                
                break;
            case ActionType.Equip:
                ToggleMore();
                break;
            case ActionType.Use:
                MessageManager.Instance.NewMessage("Item has been used");
                break;
            case ActionType.None:
                MessageManager.Instance.NewMessage("You failed to do anything hahah");
                break;
        }
    }

    private void ApplyAffinityData(List<Affinity> affinities)
    {
        affinityLayer.SetActive(true);
        int iter = 0;
        while(iter < affinities.Count  && iter < affStats.Count )
        {
            affStats[iter].Init(affinities[iter]);
            iter++;
        }
    }

    public void HideStats()
    {
        statsLayer.SetActive(false);
        IsShowing = false;
        MoreButton.text = "More";
    }
    public void ShowStats()
    {
        if (ActiveItem == null)
        {
            ActiveItem = slots[0].GetData();
            RefreshNameDescrText();
        }
        IsShowing = true;
        statsLayer.SetActive(true);
        MoreButton.text = "Less";
        RefreshStats();
    }
    public void ToggleMore()
    {
        if (!HasItems()) return;
        if (IsShowing)
        {
            HideStats();
        }
        else
        {
            ShowStats();
        }

    }

    public bool HasItems()
    {
        if (slots[0].GetData() == null)
        {
            return false;
        }
        return true;
    }
    // Use this for initialization
    void Start () {
        Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
