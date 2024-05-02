using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public Dictionary<int, (Items item, int quantity)> items = new Dictionary<int, (Items, int)>();
    public int capacity;

    public Inventory(int capacity)
    {
        this.capacity = capacity;
    }

    public void AddItem(Items item, int quantity)
    {
        if (items.ContainsKey(item.itemID)) 
        {
            items[item.itemID] = (item, items[item.itemID].quantity + quantity);
        }
        else
        {
            items.Add(item.itemID, (item, quantity));
        }
    }

    public void RemoveItem(Items item, int quantity)
    {
        if (items.ContainsKey(item.itemID))
        {
            int newQuantity = items[item.itemID].quantity - quantity;
            if (newQuantity <= 0)
            {   
                items.Remove(item.itemID);
            }
            else
            {
                items[item.itemID] = (item, newQuantity);
            }
        }
    }

    public int GetItemQuantity(Items item)
    {
        if (items.ContainsKey(item.itemID))
        {
            return items[item.itemID].quantity;
        }
        return 0;
    }

    public int GetInventoryQuantity()
    {
        return items.Count;
    }

    public bool IsFull()
    {
        return items.Count >= capacity;
    }

    public List<(Items, int)> ItemsToList()
    {
        List<(Items, int)> itemList = new List<(Items, int)>(items.Values);
        return itemList;
    }
}
