using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Inventory playerInventory;
    [Header("Inventory Settings")]
    [SerializeField] private int inventorySize = 10;

    [Header("Current Inventory")]
    [SerializeField] private List<Items> editorInventoryList;
    public List<(Items, int)> playerInventoryList;
    public List<(Items, int)> seedsInventory;

    [Space]
    [Header("Testing Assets")]
    [SerializeField] private Items draftAsset;

    private void Start()
    {
        playerInventory = new Inventory(inventorySize);
        PickUpItems(draftAsset, 1);
    }

    private void Update()
    {
        DontDestroyOnLoad(gameObject);
        playerInventoryList = InventoryToList(playerInventory);
        seedsInventory = GetItemsListOfType(Items.ItemType.Tree, playerInventory);
        editorInventoryList = SimplifiedList(playerInventoryList);
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

    public List<(Items, int)> InventoryToList(Inventory inventory)
    {
        return inventory.ItemsToList();
    }

    public List<Items> SimplifiedList(List<(Items, int)> theList)
    {
        List<Items> list = new List<Items>();
        for (int i = 0; i < theList.Count; i++)
        {
            list.Add(theList[i].Item1);
        }

        return list;
    }

    public List<(Items, int)> GetItemsListOfType(Items.ItemType type, Inventory inventory)
    {
        List<(Items, int)> allItemsList = inventory.ItemsToList();
        List<(Items, int)> finalList = new List<(Items, int)>();

        for (int i = 0; i < allItemsList.Count;i++)
        {
            if (allItemsList[i].Item1.GetItemType() == type)
            {
                finalList.Add(allItemsList[i]);
            }
            else { continue; }
        }

        return finalList;
    }
}
