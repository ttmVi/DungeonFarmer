using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Potion
{
    private string potionEffect;
    private float potionEffectDuration;
    private float potionEffectValue;
    private UnityEvent potionEffectEvent;

    public Potion(string potionEffect)
    {
        this.potionEffect = potionEffect;
    }

    public string GetPotionEffect() { return potionEffect; }

    public float GetPotionEffectDuration() { return potionEffectDuration; }

    public float GetPotionEffectValue() { return potionEffectValue; }

    public UnityEvent GetPotionEffectEvent() {  return potionEffectEvent; }
}
