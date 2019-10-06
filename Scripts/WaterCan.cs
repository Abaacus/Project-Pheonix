using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterCan : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[9];
    [Range(0,8)]
    public int waterLevel = 8;
    public Image icon;
    public const float popSize = 2f;
    public const float popDecayRate = 0.25f;

    void Awake()
    {
        waterLevel = 8;
    }

    void Update()
    {
        icon.sprite = sprites[waterLevel];
        UpdateSize();
    }

    public void IconPop()
    {
        icon.transform.localScale = Vector2.one * popSize;
        InventoryUI.instance.WaterCanEmpty();
    }

    public void UpdateSize()
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
