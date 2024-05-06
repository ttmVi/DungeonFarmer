using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Items : ScriptableObject
{
    [Header("General Item Information")]
    [SerializeField] private int itemID;
    [SerializeField] private string itemName;
    [SerializeField] private Sprite itemInventoryIcon;
    [SerializeField] private Sprite overworldSprite;
    [TextArea(3, 10)]
    [SerializeField] private string itemDescription;
    [SerializeField] private ItemType itemType;

    // Fields for seeds configuration
    [Space]
    [Header("Seed Information")]
    [SerializeField, HideInInspector] public float maxGrowthIndex;
    [SerializeField, HideInInspector] public float[] phasesGrowthIndex;
    [SerializeField, HideInInspector] public Sprite[] growingPhasesSprites;
    [SerializeField, HideInInspector] public Sprite[] deceasingSprites;
    [SerializeField, HideInInspector] public Items[] possibleDrops;

    //Fields for crops configuration
    [Space]
    [Header("Crop Information")]
    [SerializeField, HideInInspector] private string note;

    //Fields for fertilizers configuration
    [Space]
    [Header("Fertilizer Information")]
    [SerializeField, HideInInspector] public Items fertilizableSeed;

    //Fields for monster parts configuration

    //Fields for potions configuration
    [Space]
    [Header("Potion Information")]
    [SerializeField, HideInInspector] private string potionEffect;
    [SerializeField, HideInInspector] public Items[] craftingRecipe;

    public enum ItemType
    {
        General,
        Seed,
        Crop,
        Fertilizer,
        MonsterPart,
        Potion,
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

    public Tree GetSeedData()
    {
        if (itemType == ItemType.Seed)
        {
            return new Tree(itemName, maxGrowthIndex, phasesGrowthIndex, growingPhasesSprites, deceasingSprites, possibleDrops);
        }

        return null;
    }

    public Potion GetPotionData()
    {
        if (itemType == ItemType.Potion)
        {
            return new Potion(potionEffect, craftingRecipe);
        }
        return null;
    }
}
