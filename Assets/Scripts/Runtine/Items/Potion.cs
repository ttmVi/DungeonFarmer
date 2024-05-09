using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion
{
    private string potionEffect;
    private Items[] craftingRecipe;

    public Potion(string potionEffect, Items[] craftingRecipe)
    {
        this.potionEffect = potionEffect;
        this.craftingRecipe = craftingRecipe;
    }

    public string GetPotionEffect()
    {
        return potionEffect;
    }

    public Items[] GetCraftingRecipe()
    {
        return craftingRecipe;
    }
}
