using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion
{
    private string potionEffect;

    public Potion(string potionEffect)
    {
        this.potionEffect = potionEffect;
    }

    public string GetPotionEffect()
    {
        return potionEffect;
    }
}
