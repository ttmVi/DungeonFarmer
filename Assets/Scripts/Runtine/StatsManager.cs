using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject statsUI;
    [SerializeField] private GameObject healthBar;
    private int maxHealth;
    private float currentHealth;
    private int healthSlotsNumber;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            GameObject.Find("Player");
        }

        GetMaxHealth();
        healthSlotsNumber = healthBar.transform.childCount;
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = player.GetComponent<PlayerHealth>().currentHealth;
        UpdateHealthBar();
    }

    private void GetMaxHealth()
    {
        if (player != null)
        {
            maxHealth = player.GetComponent<PlayerHealth>().maxHealth;
        }
    }

    private void UpdateHealthBar()
    {
        float currentHealthRate = currentHealth / maxHealth * healthSlotsNumber;

        for (int i = 0; i < healthSlotsNumber; i++)
        {
            if (Mathf.FloorToInt(currentHealthRate) > i)
            {
                healthBar.transform.GetChild(i).GetComponent<Image>().fillAmount = 1;
            }
            else if (Mathf.FloorToInt(currentHealthRate) == i)
            {
                healthBar.transform.GetChild(i).GetComponent<Image>().fillAmount = currentHealthRate - i;
            }
            else
            {
                healthBar.transform.GetChild(i).GetComponent<Image>().fillAmount = 0;
            }
        }
    }
}
