using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCollision : MonoBehaviour
{
    public LayerMask enemyLayer;  // Set this to the enemy layer in the Inspector
    public float invincibilityTime = 0.5f;
    private Collider2D playerCollider;
    private bool isInvincible = false;
    void Start()
    {
        playerCollider = GetComponent<Collider2D>();
        if (playerCollider == null)
        {
            Debug.LogError("Player collider not found! Please attach this script to an object with a Collider2D component.");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with an enemy
        if ((enemyLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            if (!isInvincible)
            {
                StartCoroutine(DisableCollisionForSeconds(invincibilityTime));
            }
        }
    }

    IEnumerator DisableCollisionForSeconds(float seconds)
    {
        isInvincible = true;

        // Ignore collision between player and enemies
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), true);

        yield return new WaitForSeconds(seconds);

        // Re-enable collision between player and enemies
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), false);

        isInvincible = false;
    }
}
