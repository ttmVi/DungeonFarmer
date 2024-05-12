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

    private GameObject GetObjectInInteractionArea()
    {
        Collider2D[] hit = Physics2D.OverlapBoxAll(interactionPoint, new Vector2(0.5f, 0.5f), 0f);
        if (hit != null)
        {
            float closestDistance = Mathf.Infinity;
            GameObject closestObject = null;
            foreach (Collider2D hit2 in hit)
            {
                int currentPriority = 0;

                if (hit2.gameObject.TryGetComponent(out Interactable component))
                {
                    if (component.GetInteractingPriority() < currentPriority) { continue; }
                    else if (component.GetInteractingPriority() == currentPriority)
                    {
                        if (Vector2.Distance(transform.position, hit2.gameObject.transform.position) < closestDistance)
                        {
                            closestDistance = Vector2.Distance(transform.position, hit2.gameObject.transform.position);
                            closestObject = hit2.gameObject;
                        }
                        else { continue; }
                    }
                    else if (component.GetInteractingPriority() > currentPriority)
                    {
                        currentPriority = component.GetInteractingPriority();
                        closestDistance = Vector2.Distance(transform.position, hit2.gameObject.transform.position);
                        closestObject = hit2.gameObject;
                    }
                    else { continue; }
                }
                else { continue; }
            }
            return closestObject;
        }
        return null;
    }

    public GameObject GetInteractingObject()
    {
        return GetObjectInInteractionArea();
    }

    public void GetInteractionPoint()
    {
        interactionPoint = (Vector2)transform.position + facingDirection * 0.75f;
    }
}
