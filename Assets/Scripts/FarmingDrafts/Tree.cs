using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tree", menuName = "Item/Tree")]
//[Icon("Assets/Tiny RPG Forest/")]
public class Tree : Items
{
    [Header("Tree Information")]
    public int maxGrowthIndex;
    public float[] phasesGrowthIndex;
    public Sprite[] growingPhasesSprites;
    public Sprite[] deceasingSprites;

    public Tree(int itemID, string itemName, Sprite itemIcon, int treeGrowthTime, Sprite[] growingPhases, Sprite[] deceasing) : base(itemID, itemName, itemIcon)
    {
        maxGrowthIndex = treeGrowthTime;
        growingPhasesSprites = growingPhases;
        deceasingSprites = deceasing;
    }
}
