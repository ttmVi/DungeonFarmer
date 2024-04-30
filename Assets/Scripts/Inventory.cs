using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public Dictionary<int, (Item item, int quantity)> items = new Dictionary<int, (Item, int)>();

    public void AddItem(Item item, int quantity)
    {
        if (items.ContainsKey(item.id)) 
        {
            items[item.id] = (item, items[item.id].quantity + quantity);
        }
        else
        {
            items.Add(item.id, (item, quantity));
        }
    }

    public void RemoveItem(Item item, int quantity)
    {
        if (items.ContainsKey(item.id))
        {
            int newQuantity = items[item.id].quantity - quantity;
            if (newQuantity <= 0)
            {   
                items.Remove(item.id);
            }
            else
            {
                items[item.id] = (item, newQuantity);
            }
        }
    }

    public int GetItemQuantity(Item item)
    {
        if (items.ContainsKey(item.id))
        {
            return items[item.id].quantity;
        }
        return 0;
    }

    public int GetInventoryQuantity()
    {
        return items.Count;
    }

    public List<(Item, int)> ItemsToList()
    {
        List<(Item, int)> itemList = new List<(Item, int)>(items.Values);
        return itemList;
    }
}
