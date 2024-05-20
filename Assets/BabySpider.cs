using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabySpider : MonoBehaviour
{
    private EnemyHealth health;
    private EnemyAI enemyAI;
    private Vector2 startPosition;
    private Rigidbody2D rb;
    [Header("Random Movement")]
    public float waitTime;
    public float startWaitTime;
    [SerializeField, Range(0f, 2f)][Tooltip("Sets range of random movement based on local position")] public float localMinX, localMaxX;
    private float minX, maxX;
    private Transform moveSpot;
    public float patrolSpeed = 5f;
    private bool isOnCoolDown = false;
    private GroundCheck groundCheck;
    private Animator anim;
    private GameObject deathPoof;
    void Start()
    {
        deathPoof = Resources.Load<GameObject>("DeathPoof");
        groundCheck = GetComponent<GroundCheck>();
        anim = GetComponent<Animator>();
        startPosition = transform.position;
        
        health = GetComponent<EnemyHealth>();
        enemyAI = GetComponent<EnemyAI>();
        rb = GetComponent<Rigidbody2D>();
        waitTime = startWaitTime;
        minX = transform.position.x - localMinX;
        maxX = transform.position.x + localMaxX;
        moveSpot = new GameObject("SpiderMoveSpot").transform;
        moveSpot.position = new Vector2(Random.Range(minX, maxX), transform.position.y);
        //this.enemyAI.target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!enemyAI.TargetInDistance())
        {
            anim.SetTrigger("Idle");
            RandomMovement();
        }
        else if (enemyAI.TargetInDistance())
        {
            Jump();
        }

        if (health.currentHealth <= 0)
        {
            Death();
        }

        if (groundCheck.isGrounded())
        {
            anim.SetTrigger("Land");
        }
    }
    void Death()
    {
        //play death animation
        anim.SetTrigger("Death");
        Instantiate(deathPoof, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    void RandomMovement()
    {
        transform.position = Vector2.MoveTowards(transform.position, moveSpot.position, patrolSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, moveSpot.position) < 0.2f)
        {
            if (waitTime <= 0)
            {
                moveSpot.position = new Vector2(Random.Range(minX, maxX), transform.position.y);
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
    void Jump()
    {
        if (enemyAI.jumpEnabled && groundCheck.isGrounded() && !enemyAI.isInAir && !isOnCoolDown)
        {
            if (enemyAI.isInAir) return;
            anim.SetTrigger("Jump");
            enemyAI.isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, enemyAI.jumpForce);
            StartCoroutine(JumpCoolDown());
        }
    }

    IEnumerator JumpCoolDown()
    {
        isOnCoolDown = true;
        yield return new WaitForSeconds(2f);
        isOnCoolDown = false;
    }
}
