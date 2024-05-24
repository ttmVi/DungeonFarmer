using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public float currentHealth;
    private Knockback knockback;
    private PlayerMovement playerMovement;
    [Space]
    private bool isDying;
    [SerializeField] private AnimationClip dyingAnimation;
    [SerializeField] private Items healthPotion;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        knockback = GetComponent<Knockback>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (currentHealth <= 1.1f && currentHealth > 0)
        {
            if (GetComponent<PlayerInventory>().CheckForItem(healthPotion))
            {
                HealToMax();
                GetComponent<PlayerInventory>().RemoveItems(healthPotion, 1);
            }
        }
    }

    public void TakeDamage(float damage, Vector2 hitDirection)
    {
        currentHealth -= damage;
        AudioManager.instance.HurtSound.Play();
        //spawn damage particles
        //Instantiate(damageParticles, transform.position, Quaternion.identity);
        //knockback
        

        if (currentHealth <= 0)
        {
            Die();
        }
        knockback.callKnockBack(hitDirection, Vector2.up, playerMovement.directionX);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        GetComponent<PlayerAnimationController>().TriggerHurtingAnimation();
        //spawn damage particles
        //Instantiate(damageParticles, transform.position, Quaternion.identity);
        //knockback

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (!isDying)
        {
            StartCoroutine(Dying());
        }
    }

    private IEnumerator Dying()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<PlayerAnimationController>().TriggerDyingAnimation();
        yield return new WaitForSeconds(dyingAnimation.length);

        GetComponent<PlayerInventory>().RemoveRandomDungeonItems();
        FindObjectOfType<GameManager>().ToFarm(null);
        currentHealth = maxHealth;
        //GetComponent<BoxCollider2D>().enabled = true;
        //isDying = false;
        yield return null;
    }

    public void StopDying() { isDying = false; }

    public bool IsDying() { return isDying; }

    public void Heal(float hpPoint) 
    { 
        GetComponent<PlayerAnimationController>().TriggerHealingAnimation();
        currentHealth += hpPoint; 
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void HealToMax() 
    { 
        currentHealth = maxHealth;
        GetComponent<PlayerAnimationController>().TriggerHealingAnimation();
    }
}
