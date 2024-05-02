using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Inventory playerInventory;
    [Header("Inventory Settings")]
    [SerializeField] private int inventorySize = 10;

    private void Start()
    {
        playerInventory = new Inventory(inventorySize);
    }

    private void Update()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void PickUpItems(Items item, int quantity)
    {
        if (playerInventory.IsFull())
        {
            Debug.Log("Inventory is full");
            return;
        }
        else
        {
            playerInventory.AddItem(item, quantity);
        }
    }

    public void RemoveItems(Items item, int quantity)
    {
        playerInventory.RemoveItem(item, quantity);
    }
}
