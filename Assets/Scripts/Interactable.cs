using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    //private PlayerInventory playerInstance;
    [SerializeField] private UnityEvent interactEvent;

    // Start is called before the first frame update
    void Start()
    {
        //playerInstance = GameObject.Find("player").GetComponent<PlayerInventory>();
    }

    public void IsInteracted()
    {
        if (interactEvent != null)
        {
            interactEvent.Invoke();
        }
    }

    public void PickUp(Item item, int quantity)
    {
        /*if (playerInstance.CheckForEmptySlots(item))
        {
            playerInstance.PickUpItems(item, quantity);
            Destroy(gameObject);
        }
        else
        {
            playerInstance.gameObject.GetComponent<DialoguesTrigger>().TriggerDialogues(0);
            Debug.Log("No empty slots");
        }*/
    }
}
