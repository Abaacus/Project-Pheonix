using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Multiple instances of inventory found");
        }
        instance = this;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }
    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public InventorySlot[] slots;
    public Transform itemsParent;

    public WaterCan waterCan;
    public List<Item> items = new List<Item>();

    public int inventorySize = 2;
    public bool inventoryFull;
    public int indexAdded;

    public bool Add(Item item)
    {
        if (!item.isDefaultItem)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (item = items[i])
                {
                    if (slots[i].itemCount < items[i].itemStackSize)
                    {
                        slots[i].itemCount++;
                        indexAdded = i;
                        onItemChangedCallback.Invoke();
                        return true;
                    }
                }
            }

            if (items.Count < inventorySize)
            {
                int i = items.Count;
                items.Add(item);
                slots[i].itemCount++;
                indexAdded = items.Count - 1;
                onItemChangedCallback.Invoke();
                return true;
            }
        }

        inventoryFull = true;
        onItemChangedCallback.Invoke();
        return false;
    }

    public void Remove(Item item)
    {
        items.Remove(item);

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }

    public void UpdateInventoryUI()
    {
        onItemChangedCallback.Invoke();
    }
}
