using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Items : ScriptableObject
{
    [Header("General Item Information")]
    private int itemID;
    private string itemName;
    private Sprite itemInventoryIcon;
    private Sprite overworldSprite;
    [TextArea(3, 10)]
    private string itemDescription;
    private ItemType itemType;

    // Fields for trees configuration
    [Space]
    [Header("Tree Information")]
    [SerializeField, HideInInspector] public float maxGrowthIndex;
    [SerializeField, HideInInspector] public float[] phasesGrowthIndex;
    [SerializeField, HideInInspector] public Sprite[] growingPhasesSprites;
    [SerializeField, HideInInspector] public Sprite[] deceasingSprites;
    [SerializeField, HideInInspector] public Items[] possibleDrops;

    public enum ItemType
    {
        General,
        Tree
    }

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

    public ItemType GetItemType()
    {
        return itemType;
    }

    public Tree GetTreeData()
    {
        if (itemType == ItemType.Tree)
        {
            return new Tree(itemName, maxGrowthIndex, phasesGrowthIndex, growingPhasesSprites, deceasingSprites, possibleDrops);
        }

        return null;
    }
}
