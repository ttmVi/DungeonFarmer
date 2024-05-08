using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fertilizer
{
    private Items fertilizableSeed;
    private Items[] fertilizerCraftingRecipe;

    public Fertilizer(Items fertilizableSeed, Items[] recipe)
    {
        this.fertilizableSeed = fertilizableSeed;
        fertilizerCraftingRecipe = recipe;
    }

    public Items[] GetCraftingRecipe() { return fertilizerCraftingRecipe; }
}
