using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 3f;

    bool isFocus;
    Transform player;

    bool hasInteracted;

    public virtual void Interact()
    {
        // this method will be overwritten
        Debug.Log("Interacting with " + transform);
    }

    private void Update()
    {
        if (isFocus && !hasInteracted)
        {
            float distance = Vector3.Distance(player.position, transform.position);
            if (distance <= radius)
            {
                Interact();
                hasInteracted = true;
            }
        }
    }

    public void OnFocused (Transform playerTransform)
    {
        hasInteracted = false;
        isFocus = true;
        player = playerTransform;
    }

    public void OnDefocused()
    {
        hasInteracted = false;
        isFocus = false;
        player = null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
