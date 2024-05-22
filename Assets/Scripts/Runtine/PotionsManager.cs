using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionsManager : MonoBehaviour
{
    [SerializeField] private GameObject player;

    public void ActivateHealthPotion(Items potion)
    {
        Potion potionData = potion.GetPotionData();
        player.GetComponent<PlayerHealth>().HealToMax();
    }

    public void ActivateRagePotion(Items potion)
    {
        StartCoroutine(Raging(potion));
    }

    private IEnumerator Raging(Items potion)
    {
        Potion potionData = potion.GetPotionData();
        Melee weapon = player.transform.GetChild(0).GetComponent<Melee>();
        int originalDamage = weapon.GetDamagePower();
        int newDamage = Mathf.RoundToInt(originalDamage * potionData.GetPotionEffectValue());
        float effectiveDuration = potionData.GetPotionEffectDuration();

        player.transform.GetChild(0).GetComponent<Melee>().ChangeDamagePower(newDamage);
        yield return new WaitForSeconds(effectiveDuration);
        player.transform.GetChild(0).GetComponent<Melee>().ChangeDamagePower(originalDamage);
    }
}
