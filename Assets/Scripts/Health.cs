using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health Configuration")]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float health;
    
    public virtual void IsAttacked(float dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            Destroyed();
        }
    }

    public virtual void Destroyed()
    {
        Destroy(gameObject);
    }
}
