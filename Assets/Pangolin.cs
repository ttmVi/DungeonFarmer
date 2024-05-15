using Pathfinding.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pangolin : MonoBehaviour
{
    private EnemyAI enemyAI;
    private bool lookingOut = false;
    public GameObject projectile;
    private bool shooting = false;
    public float projectileSpeed = 5f;
    private EnemyHealth health;
    private void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
        health = GetComponent<EnemyHealth>();
    }
    private void Update()
    {
        if (!enemyAI.TargetInDistance()&&!lookingOut)
        {
            StartCoroutine(LookOut());
        }

        if(enemyAI.TargetInDistance())
        {
            LookAtPlayer();
            BeginAttack();
        }

        if (health.currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(this.gameObject);
    }
    void LookAtPlayer()
    {
        if (transform.position.x>enemyAI.target.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (transform.position.x < enemyAI.target.position.x)
        {
            transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
    IEnumerator LookOut()
    {
        lookingOut = true;
        while(!enemyAI.TargetInDistance())
        {
            //look back and forth
            transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            yield return new WaitForSeconds(1f);
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            yield return new WaitForSeconds(1f);
        }
        lookingOut = false;
    }
    void BeginAttack()
    {
        if (!shooting)
        {
            StartCoroutine(Attack());
        }
    }
    IEnumerator Attack() 
    { 
        shooting = true;
        Vector2 direction = (enemyAI.target.transform.position - transform.position).normalized;
        GameObject enemyProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
        enemyProjectile.GetComponent<EnemyProjectile>().Initialize(direction,projectileSpeed);
        yield return new WaitForSeconds(1f);
        shooting = false;
    }
}