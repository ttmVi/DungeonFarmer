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
        if (items.ContainsKey(item.GetItemID())) 
        {
            items[item.GetItemID()] = (item, items[item.GetItemID()].quantity + quantity);
        }
        else
        {
            items.Add(item.GetItemID(), (item, quantity));
        }
    }

    public void RemoveItem(Items item, int quantity)
    {
        if (items.ContainsKey(item.GetItemID()))
        {
            int newQuantity = items[item.GetItemID()].quantity - quantity;
            if (newQuantity <= 0)
            {   
                items.Remove(item.GetItemID());
            }
            else
            {
                items[item.GetItemID()] = (item, newQuantity);
            }
        }
    }

    public int GetItemQuantity(Items item)
    {
        if (items.ContainsKey(item.GetItemID()))
        {
            return items[item.GetItemID()].quantity;
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
