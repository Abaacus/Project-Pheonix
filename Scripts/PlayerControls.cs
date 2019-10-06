using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControls : MonoBehaviour
{
    public Camera cam;
    public Transform targetArea;

    public ButtonMaster clickMaster;

    public Animator animator;
    Vector3 targetPoint = Vector3.zero;
    Transform target;
    public float playerSpeed = 5f;
    public float targetAreaDistanceFromPlayer = 1f;
    bool flipped;
    bool inMotion;

    public Interactable focus;
    bool autoNaviagte;

    public float interactDelay = 5f;
    float lastInteract;

    public bool itemPickedUp;
    ContactFilter2D contactFilter = new ContactFilter2D();
    List<Collider2D> selectedObjects;

    private void Start()
    {
        contactFilter.NoFilter();
        selectedObjects = new List<Collider2D>();
        cam = Camera.main;
        clickMaster = ButtonMaster.instance;
    }

    private void Update()
    {
        MovePlayerWASD();
        SetAnimation();
        MoveSelectedArea();
        KeyBoardInteract();
    }

    #region Movement
    void MovePlayerMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            autoNaviagte = true;
            targetPoint = mousePos2D;
            RemoveFocus();
        }

        if (target != null)
        {
            targetPoint = target.position;
        }

        MoveToPoint(targetPoint);
    }

    void MovePlayerWASD()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        FlippedMovement(direction);

        if (direction != Vector3.zero)
        {
            Vector3 moveVelocity = direction * playerSpeed;
            transform.position += moveVelocity;
            inMotion = true;
            autoNaviagte = false;
            RemoveFocus();
        }
        else
        {
            inMotion = false;
        }

    }

    void MoveToPoint(Vector3 point)
    {
        if (autoNaviagte)
        {
            if (target != null)
            {
                autoNaviagte &= Vector3.Distance(transform.position, point) >= target.GetComponent<Interactable>().radius * .8f;
            }
            else
            {
                if (Vector3.Distance(transform.position, point) < 2f * playerSpeed)
                {
                    transform.position = point;
                    autoNaviagte = false;
                }
            }

            Vector3 direction = transform.position - point;
            FlippedMovement(-direction);
            transform.position -= direction.normalized * playerSpeed;
            inMotion = true;
        }
        autoNaviagte &= transform.position != targetPoint;
    }

    void MoveSelectedArea()
    {
        Vector3 newPos = Vector3.one * -1f;
        if (flipped)
        {
            newPos.x = Mathf.Round(transform.position.x - targetAreaDistanceFromPlayer);
        }
        else
        {
            newPos.x = Mathf.Round(transform.position.x + targetAreaDistanceFromPlayer);
        }
        newPos.y = Mathf.Round(transform.position.y);
        targetArea.position = newPos;
    }

    void FlippedMovement(Vector3 direction)
    {
        if (direction.x > 0)
        {
            transform.GetComponent<SpriteRenderer>().flipX = false;
            flipped = false;
        }
        else if (direction.x < 0)
        {
            transform.GetComponent<SpriteRenderer>().flipX = true;
            flipped = true;
        }
    }

    void SetAnimation()
    {
        animator.SetBool("isMoving", inMotion);
    }

    #endregion


    #region Targeting

    void SelectObject()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent(out Interactable interactable))
                {
                    SetFocus(interactable);
                }
                else
                {
                    selectedObjects.Clear();
                    selectedObjects[0] = hit.collider;
                    Interaction(true, mousePos);
                }
            }
        }
    }

    void FollowTarget(Interactable newTarget)
    {
        target = newTarget.transform;
        autoNaviagte = true;
    }

    void StopFollowingTarget()
    {
        target = null;
    }

    void SetFocus(Interactable newFocus)
    {
        if (newFocus != focus)
        {
            if (focus != null)
            {
                RemoveFocus();
            }
        }

        focus = newFocus;
        focus.OnFocused(transform);
        FollowTarget(newFocus);
    }

    void RemoveFocus()
    {
        if (focus != null)
        {
            focus.OnDefocused();
        }

        focus = null;
        StopFollowingTarget();
    }
    #endregion


    #region Interaction

    void KeyBoardInteract()
	{
        if (Input.GetButton("Jump"))
        {
            if (Time.time > lastInteract)
            {
                lastInteract = Time.time + interactDelay;
                selectedObjects.Clear();
                Physics2D.OverlapCollider(targetArea.GetComponent<Collider2D>(), contactFilter, selectedObjects);

                Debug.Log("Interacting");

                if (clickMaster.currentObj != null)
                {
                    switch (clickMaster.currentObj.parent.name)
                    {

                        case "Slot 1":
                            Debug.Log("Using item in slot1");
                            break;

                        case "Slot 2":
                            Debug.Log("Using item in slot2");
                            break;

                        case "Tool Slot":
                            Debug.Log("Using item in toolSlot");
                            break;
                    }
                }
                else
                {
                    Debug.Log("Using hands");
                }

                Interaction(false, Vector2.zero);
            }
        }
	}

    void Interaction(bool clicked, Vector3 mousePos)
    {
        itemPickedUp = false;
        foreach (var selected in selectedObjects)
        {
            if (selected.TryGetComponent(out ItemPickup itemPickup))
            {
                itemPickup.Interact();
                itemPickedUp = true;
            }
            if (!itemPickedUp)
            {
                Debug.Log("Object Selected: " + selected.name);
                if (clickMaster.currentObj != null)
                {
                    if (clickMaster.currentObj.parent.TryGetComponent(out InventorySlot slot))
                    {
                        if (slot.itemCount > 0)
                        {
                            if (clicked)
                            {
                                mousePos.z = transform.position.z;
                                if (Vector2.Distance(transform.position, mousePos) > 1f)
                                {
                                    targetPoint = mousePos;
                                    Instantiate(slot.item.plantObject, mousePos, Quaternion.identity);
                                }
                                else
                                {
                                    Instantiate(slot.item.plantObject, mousePos, Quaternion.identity);
                                }
                            }
                            else
                            {
                                Instantiate(slot.item.plantObject, targetArea.position, Quaternion.identity);
                            }

                            slot.RemoveItem();
                        }
                    }

                    if (clickMaster.currentObj.parent.name == "Tool Slot")
                    {
                        if (selected.tag == "Water")
                        {
                            Debug.Log("Filling Can");
                            WaterCan waterCan = clickMaster.currentObj.parent.GetComponent<WaterCan>();
                            if (waterCan.waterLevel < 8)
                            {
                                waterCan.waterLevel++;
                            }
                        }

                        if (selected.tag == "Plant" && selected.TryGetComponent(out Plant plant))
                        {
                            if (plant.plantState == Plant.PlantState.thirsty)
                            {
                                Debug.Log("Watering Plant");
                                WaterCan waterCan = clickMaster.currentObj.parent.GetComponent<WaterCan>();
                                if (waterCan.waterLevel > 0)
                                {
                                    plant.OnWater();
                                    waterCan.waterLevel--;
                                }
                                else
                                {
                                    waterCan.IconPop();
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    #endregion
}
