using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonMaster : MonoBehaviour
{
    public static ButtonMaster instance;
    public ClickableObject[] clickableObjs;
    public Transform currentObj;
    public TextMeshProUGUI instructions;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Multiple instances of ClickMaster found");
        }
        instance = this;

        clickableObjs = FindObjectsOfType<ClickableObject>();
    }

    void LateUpdate()
    {
        UpdateSelectedObjects();
        if (Time.time > 20f)
        {
            instructions.enabled = false;
        }
    }

    void UpdateSelectedObjects()
    {
        foreach (var obj in clickableObjs)
        {
            if (obj.buttonSelected)
            {
                if (currentObj != null)
                {

                    if (obj.transform != currentObj)
                    {
                        currentObj.GetComponent<ClickableObject>().buttonSelected = false;
                        currentObj = obj.transform;
                    }
                }
                else
                {
                    currentObj = obj.transform;
                }
            }
        }
    }
}
