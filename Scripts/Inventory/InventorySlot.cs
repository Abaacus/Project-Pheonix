using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public TextMeshProUGUI itemCountDisplay;
    public int itemCount;
    public Image icon;
    public const float popSize = 2f;
    public const float popDecayRate = 0.25f;
    public Item item;

    public void AddItem (Item newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
        UpdateCountDisplay();
    }

    public void RemoveItem()
    {
        itemCount--;
        if (itemCount == 0)
        {
            ClearSlot();
        }

        UpdateCountDisplay();
    }

    public void ClearSlot()
    {
        item = null;
        itemCount = 0;
        icon.sprite = null;
        icon.enabled = false;
    }

    void UpdateCountDisplay()
    {
        if (itemCount <= 1)
        {
            itemCountDisplay.text = "";
            itemCountDisplay.enabled = false;
        }
        else
        {
            itemCountDisplay.text = "x" + itemCount;
            itemCountDisplay.enabled = true;
        }
    }

    public void SelectItem()
    {
        if (item != null)
        {
            item.Use();
        }
    }

    public void IconPop()
    {
        icon.transform.localScale = Vector2.one * popSize;
    }

    void Update()
    {
        if (icon.transform.localScale.x > popDecayRate && icon.transform.localScale.x > 1)
        {
            Vector2 iconScale = new Vector2(icon.transform.localScale.x - popDecayRate, icon.transform.localScale.y - popDecayRate);
            icon.transform.localScale = iconScale;
        }
        else
        {
            icon.transform.localScale = Vector3.one;
        }
    }
}
