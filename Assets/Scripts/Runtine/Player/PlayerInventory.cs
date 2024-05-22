using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Inventory playerInventory;
    public Inventory dungeonTempInventory;

    [Header("Inventory Settings")]
    [SerializeField] private int inventorySize = 10;

    [Header("Current Inventory")]
    [SerializeField] private List<Items> editorInventoryList;
    public List<(Items, int)> playerInventoryList;
    public List<(Items, int)> seedsInventory;
    public List<(Items, int)> fertilizersInventory;

    [SerializeField] private List<Items> dungeonInventoryList;

    [Space]
    [Header("Testing Assets")]
    [SerializeField] private Items[] draftAssets;

    private void Start()
    {
        playerInventory = new Inventory(inventorySize);
        dungeonTempInventory = new Inventory(inventorySize);
        foreach (var item in draftAssets)
        {
            PickUpItems(item, 1);
        }
    }

    private void Update()
    {
        playerInventoryList = InventoryToList(playerInventory);
        seedsInventory = GetItemsListOfType(Items.ItemType.Seed, playerInventory);
        fertilizersInventory = GetItemsListOfType(Items.ItemType.Fertilizer, playerInventory);
        editorInventoryList = SimplifiedList(playerInventoryList);
        dungeonInventoryList = SimplifiedList(InventoryToList(dungeonTempInventory));
    }

    public void LoadInventory(Inventory inventory)
    {
        playerInventory = inventory;
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

    public void PickUpItemsInDungeon(Items item, int quantity)
    {
        if (dungeonTempInventory.IsFull())
        {
            Debug.Log("Inventory is full");
            return;
        }
        else
        {
            dungeonTempInventory.AddItem(item, quantity);
        }
    }

    public void RemoveRandomDungeonItems()
    {
        for (int i = 0; i < dungeonTempInventory.ItemsToList().Count; i++)
        {
            int dropOrKeep = Random.Range(0, 2);
            if (dropOrKeep == 0)
            {
                dungeonTempInventory.RemoveItem(dungeonTempInventory.ItemsToList()[i].Item1, dungeonTempInventory.ItemsToList()[i].Item2);
            }
            else { continue; }
        }
    }

    public void StoreItemsFromDungeon()
    {
        StartCoroutine(StartStoring());
    }

    private IEnumerator StartStoring()
    {
        for (int i = 0; i < dungeonTempInventory.ItemsToList().Count; i++)
        {
            PickUpItems(dungeonTempInventory.ItemsToList()[i].Item1, dungeonTempInventory.ItemsToList()[i].Item2);
            yield return new WaitForEndOfFrame();
        }

        dungeonTempInventory = null;
        yield return null;
    }

    public void RemoveItems(Items item, int quantity)
    {
        playerInventory.RemoveItem(item, quantity);
    }

    public int GetItemQuantity(Items item)
    {
        return playerInventory.GetItemQuantity(item);
    }

    public bool CheckForItem(Items item)
    {
        if (playerInventory.GetItemQuantity(item) > 0)
        {
            return true;
        }
        return false;
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

    public void FetchWater(Items emptyFetchingBottle, Items filledFetchingBottle)
    {
        if (CheckForItem(emptyFetchingBottle))
        {
            playerInventory.AddItem(filledFetchingBottle, 1);
            playerInventory.RemoveItem(emptyFetchingBottle, 1);
        }
        else { }
    }

    public void EmptyWaterBottle(Items filledFetchingBottle, Items emptyFetchingBottle)
    {
        if (CheckForItem(filledFetchingBottle))
        {
            playerInventory.AddItem(emptyFetchingBottle, 1); 
            playerInventory.RemoveItem(filledFetchingBottle, 1);
        }
        else { Debug.Log("Bottle already empty"); }
    }
}
