using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBoss : MonoBehaviour
{
    private EnemyHealth health;
    private EnemyAI enemyAI;
    private Rigidbody2D rb;
    private Animator anim;
    private float idleTime = 2.0f;
    public int attackCounter = 0;
    public GameObject rock;
    public GameObject projectile;
    public float projectileSpeed = 5f;
    // Start is called before the first frame update
    private enum State
    {
        Attacking,
        Idle,
        Hurt,
        Slamming,
        Sucking
    }

    private State currentState;
    private bool canSlam = true;
    private bool canSuck = true;
    private bool canBite = true;
    private bool canIdle = true;
    private bool canHurt = true;

    void Start()
    {
        anim = GetComponent<Animator>();
        health = GetComponent<EnemyHealth>();
        enemyAI = GetComponent<EnemyAI>();
        rb = GetComponent<Rigidbody2D>();
        currentState = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyAI.TargetInDistance())
        {
            switch (currentState)
            {
                case State.Attacking:
                    HandleAttacking();
                    break;
                case State.Idle:
                    HandleIdle();
                    break;
                case State.Hurt:
                    HandleHurt();
                    break;
                case State.Slamming:
                    HandleSlamming();
                    break;
                case State.Sucking:
                    HandleSuck();
                    break;
            }
        }
        else
        {
            currentState = State.Idle;
        }

        if(health.currentHealth <=50)
        {
            enemyAI.speed = 1.0f;
        }

        if(health.currentHealth <= 0)
        {
            anim.SetTrigger("Death");
            Destroy(this.gameObject);
        }
        
    }

    void HandleAttacking()
    {
        enemyAI.speed = 0f;
        anim.ResetTrigger("Idle");
        if (canBite)
        {
            StartCoroutine(Bite());
        }

    }

    void HandleIdle()
    {
        if (canIdle)
        {
            StartCoroutine(Idle());
        }
    }

    void HandleHurt()
    {
        // Trigger hurt animation
        Debug.Log("Hurt...");
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            anim.SetTrigger("Hurt");
            currentState = State.Idle;
        }
          // Return to Idle after being hurt
    }
    void HandleSlamming()
    {
        enemyAI.speed = 0f;
        anim.ResetTrigger("Idle");
        Debug.Log("enter Slamming...");
        if (canSlam)
        {
            StartCoroutine(Slam());
        }
    }
    void HandleSuck()
    {
        enemyAI.speed = 0f;
        anim.ResetTrigger("Idle");
        Debug.Log("enter Sucking...");
        if (canSuck)
        {
            StartCoroutine(Suck());
        }
    }

    IEnumerator Slam()
    {
        canSlam = false;
        // Wait for the transition to end
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);

        // Do some action
        Debug.Log("Slamming...");
        anim.SetTrigger("Slam");
        
        // Wait for the animation to end
        yield return new WaitWhile(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);

        // Do some action
        int numRocks = Random.Range(12, 18);
        for (int i = 0; i < numRocks; i++)
        {
            StartCoroutine(SpawnRock());
            yield return new WaitForSecondsRealtime(0.2f);
            //DropRocks();
        }
        currentState = State.Idle;
        canSlam = true;
    }
    IEnumerator Suck()
    {
        canSuck = false;
        // Wait for the transition to end
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);

        // Do some action
        Debug.Log("Sucking...");
        anim.SetTrigger("Suck");
        // Spawn projectiles
        yield return new WaitWhile(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);
        yield return new WaitForSecondsRealtime(0.6f);
        for (int i = 0; i < 6; i++)
        {
            Shoot();
            yield return new WaitForSecondsRealtime(0.5f);
        }
        // Wait for the animation to end
        yield return new WaitForSecondsRealtime(1.0f);
        anim.SetTrigger("Tired");
        yield return new WaitForSecondsRealtime(1.0f);
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);

        // Do some action
        currentState = State.Idle;
        canSuck = true;
    }
    void Shoot()
    {
        Vector2 direction = (enemyAI.target.transform.position - transform.position).normalized;
        GameObject enemyProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
        enemyProjectile.GetComponent<EnemyProjectile>().Initialize(direction, projectileSpeed);
    }
    IEnumerator SpawnRock()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        float randomNumX = Random.Range(-3, 3);
        float randomNumY = Random.Range(4, 7);
        Vector2 rockSpawn = new Vector2(enemyAI.target.transform.position.x+randomNumX, enemyAI.target.transform.position.y+randomNumY);
        Instantiate(rock, rockSpawn, Quaternion.identity);
    }
    IEnumerator Bite()
    {
        canBite = false;
        // Wait for the transition to end
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);

        // Do some action
        Debug.Log("Biting...");
        anim.SetTrigger("Bite");

        // Wait for the animation to end
        yield return new WaitWhile(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);

        currentState = State.Idle;
        canBite = true;
    }

    IEnumerator Idle()
    {
        canIdle = false;
        // Wait for the transition to end
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);

        // Do some action
        Debug.Log("Idling...");
        anim.SetTrigger("Idle");

        // Wait for the animation to end
        //yield return new WaitWhile(() => anim.GetCurrentAnimatorStateInfo(0).IsName("SnakeBossIdle"));

        Debug.Log("Switching State");
        //select a random state
        int randomState = Random.Range(0, 3);
        if (randomState == 0)
        {
            currentState = State.Attacking;
        }
        else if (randomState == 1)
        {
            currentState = State.Slamming;
        }
        else
        {
            currentState = State.Sucking;
        }
        canIdle = true;
    }
}
