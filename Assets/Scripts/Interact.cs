using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] private KeyCode[] interactionKeys;

    private Vector2 interactionPoint;
    public Vector2 facingDirection;

    //[SerializeField] private InventoryUI inventoryUI;
    //[SerializeField] private DialogueManager dialogueManager;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            facingDirection = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            facingDirection = Vector2.right;
        }
        else
        {
            //facingDirection = Vector2.down;
        }

        GetInteractionPoint();
        Debug.DrawLine(transform.position, interactionPoint);

        if (interactionPoint != null)
        {
            for (int i = 0; i < interactionKeys.Length; i++)
            {
                if (Input.GetKeyDown(interactionKeys[i]))
                {
                    //Debug.Log(GetObjectInInteractionArea().name);
                    GetObjectInInteractionArea().TryGetComponent(out Interactable interactable);
                    if (interactable != null && interactable.enabled)
                    {
                        interactable.IsInteracted();
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
                if (hit2.gameObject.name == "border" || hit2.gameObject.name == "player")
                {
                    continue;
                }
                else if (!hit2.gameObject.TryGetComponent(out Interactable component)) { continue; }
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
