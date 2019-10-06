using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public GameObject prefab;
	public GameObject plantObject;
    public Sprite icon;
    public bool isDefaultItem;
    public int itemStackSize = 20;

    public virtual void Use()
    {

        Debug.Log("Using " + name);
    }
}
