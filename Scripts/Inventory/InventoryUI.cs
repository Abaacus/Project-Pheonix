using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    #region Singleton
    public static InventoryUI instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Multiple instances of inventoryUI found");
        }
        instance = this;
    }
    #endregion

    public TextMeshProUGUI statusText;
    public Transform itemsParent;
    Inventory inventory;
    InventorySlot[] slots;

    public float textDelayTime = 5f;
    float textEndTime;

    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = inventory.slots;
    }

    void Update()
    {
        statusText.enabled &= Time.time <= textEndTime;
    }

    void UpdateUI()
    {
        if (inventory.inventoryFull)
        {
            InventoryFull();
        }
        else
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (i < inventory.items.Count)
                {
                    slots[i].AddItem(inventory.items[i]);
                    ItemAdded(slots[i]);
                    if (i == inventory.indexAdded)
                    {
                        slots[i].IconPop();
                    }
                }
                else
                {
                    slots[i].ClearSlot();
                }
            }
        }
    }

    void InventoryFull()
    {
        foreach (var slot in slots)
        {
            slot.IconPop();
        }
        statusText.text = "Inventory    Full";
        statusText.enabled = true;
        textEndTime = Time.time + textDelayTime;
    }

    public void WaterCanEmpty()
    {
        statusText.text = "Watering    Can    Empty";
        statusText.enabled = true;
        textEndTime = Time.time + textDelayTime;
    }

    void ItemAdded(InventorySlot slot)
    {
        statusText.text = slot.item.name + "    x" + slot.itemCount;
        statusText.enabled = true;
        textEndTime = Time.time + textDelayTime;
    }
}
