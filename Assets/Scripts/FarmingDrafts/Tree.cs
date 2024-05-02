using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree
{
    [Header("Tree Information")]
    public string treeName;
    public float maxGrowthIndex;
    public float[] phasesGrowthIndex;
    public Sprite[] growingPhasesSprites;
    public Sprite[] deceasingSprites;

    [Header("Possible Drops")]
    public Items[] possibleDrops;

    public Tree(string treeName, float maxGrowthIndex, float[] phasesGrowthIndex, Sprite[] growingPhasesSprites, Sprite[] deceasingSprites, Items[] possibleDrops)
    {
        this.treeName = treeName;
        this.maxGrowthIndex = maxGrowthIndex;
        this.phasesGrowthIndex = phasesGrowthIndex;
        this.growingPhasesSprites = growingPhasesSprites;
        this.deceasingSprites = deceasingSprites;
        this.possibleDrops = possibleDrops;
    }
}
