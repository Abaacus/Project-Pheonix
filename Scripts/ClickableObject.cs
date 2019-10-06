using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickableObject : MonoBehaviour, IPointerClickHandler
{
    public Transform player;
    public bool buttonSelected;
    public Sprite[] sprites = new Sprite[2];
    public const float dropRadius = 1f;
    public KeyCode key;

    void Update()
    {
        if (buttonSelected)
        {
            OnSelected();
        }
        else
        {
            OnDeselected();
        }

        if (Input.GetKeyDown(key))
        {
            if (!buttonSelected)
            {
                buttonSelected = true;
            }
            else
            {
                buttonSelected = false;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                Debug.Log("Left click on " + transform.parent);
                if (!buttonSelected)
                {
                    buttonSelected = true;
                }
                else
                {
                    buttonSelected = false;
                }
                break;

            case PointerEventData.InputButton.Right:
                Debug.Log("Right click on " + transform.parent);
                if (transform.parent.TryGetComponent(out InventorySlot slot))
                {
                    slot.itemCount--;
                    float ringRadius = dropRadius;
                    float rndAngle = Random.value * 6.28f;
                    float cX = Mathf.Sin(rndAngle);
                    float cY = Mathf.Cos(rndAngle);

                    Vector3 dropPos = new Vector3(cX, cY, 0);
                    dropPos *= ringRadius;
                    dropPos += player.position;

                    Instantiate(slot.item.prefab, dropPos, Quaternion.identity);

                    slot.RemoveItem();
                }
                break;
        }
    }

    public void OnSelected()
    {
        transform.GetComponent<Image>().sprite = sprites[1];
    }

    public void OnDeselected()
    {
        transform.GetComponent<Image>().sprite = sprites[0];
    }
}