using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private KeyCode[] interactionKeys;

    private Vector2 interactionPoint;
    public Vector2 facingDirection;

    //[SerializeField] private InventoryUI inventoryUI;
    //[SerializeField] private DialogueManager dialogueManager;

    // Update is called once per frame
    void Update()
    {
        facingDirection = new Vector2(transform.localScale.x, 0);

        GetInteractionPoint();
        Debug.DrawLine(transform.position, interactionPoint);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (interactionPoint != null)
            {
                if (GetObjectInInteractionArea() != null)
                {
                    Debug.Log(GetObjectInInteractionArea().name);
                    if (GetObjectInInteractionArea().TryGetComponent(out Interactable interactable))
                    {
                        if (interactable != null && interactable.enabled)
                        {
                            interactable.IsInteracted();
                        }
                    }
                }
            }
        }
    }

    public GameObject GetObjectInInteractionArea()
    {
        Collider2D[] hit = Physics2D.OverlapBoxAll(interactionPoint, new Vector2(0.5f, 0.5f), 0f);
        if (hit != null)
        {
            float closestDistance = Mathf.Infinity;
            GameObject closestObject = null;
            foreach (Collider2D hit2 in hit)
            {
                if (!hit2.gameObject.TryGetComponent(out Interactable component)) { continue; }
                else if (Vector2.Distance(transform.position, hit2.gameObject.transform.position) < closestDistance)
                {
                    closestObject = hit2.gameObject;
                }
            }
            return closestObject;
        }
        return null;
    }

    public void GetInteractionPoint()
    {
        interactionPoint = (Vector2)transform.position + facingDirection * 0.75f;
    }
}
