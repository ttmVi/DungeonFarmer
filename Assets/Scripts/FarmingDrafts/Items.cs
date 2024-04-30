using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/General Item")]
public class Items : ScriptableObject
{
    [Header("General Item Information")]
    public int itemID;
    public string itemName;
    public Sprite itemInventoryIcon;
    public Sprite overworldSprite;
    [TextArea(3, 10)]
    public string itemDescription;

    public Items(int itemID, string itemName, Sprite itemIcon)
    {
        this.itemID = itemID;
        this.itemName = itemName;
        this.itemInventoryIcon = itemIcon;
    }

    // Getting item's information
    public int GetItemID()
    {
        return itemID;
    }

    public string GetItemName()
    {
        return itemName;
    }

    public Sprite GetItemIcon()
    {
        return itemInventoryIcon;
    }

    public Sprite GetItemOverworldSprite()
    {
        return overworldSprite;
    }

    public string GetItemDescription()
    {
        return itemDescription;
    }
}
