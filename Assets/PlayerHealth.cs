using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;
    private Knockback knockback;
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        knockback = GetComponent<Knockback>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, Vector2 hitDirection)
    {
        currentHealth -= damage;
        //spawn damage particles
        //Instantiate(damageParticles, transform.position, Quaternion.identity);
        //knockback
        

        if (currentHealth <= 0)
        {
            //Die();
        }
        knockback.callKnockBack(hitDirection, Vector2.up, playerMovement.directionX);
    }
}
