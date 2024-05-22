using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    private EnemyHealth health;
    private EnemyAI enemyAI;
    private Vector2 startPosition;
    private Rigidbody2D rb;
    public GameObject babySpiderPrefab;
    [Header("Random Movement")]
    public float waitTime;
    public float startWaitTime;
    [SerializeField, Range(0f, 2f)][Tooltip("Sets range of random movement based on local position")] public float localMinX, localMaxX;
    private float minX, maxX;
    private Transform moveSpot;
    public float patrolSpeed = 5f;
    private bool isOnCoolDown = false;
    private GroundCheck groundCheck;
    public float checkInterval = 0.05f;  // Time interval between ground checks
    public float maxFallDistance = 10f;  // Maximum distance to fall before giving
    private Animator anim;
    private bool inAir;
    public GameObject deathPoof;
    [SerializeField] private Items[] droppingItems;
    void Start()
    {
        deathPoof = Resources.Load<GameObject>("DeathPoof");
        startPosition = transform.position;
        groundCheck = GetComponent<GroundCheck>();
        health = GetComponent<EnemyHealth>();
        enemyAI = GetComponent<EnemyAI>();
        rb = GetComponent<Rigidbody2D>();
        waitTime = startWaitTime;
        minX = transform.position.x - localMinX;
        maxX = transform.position.x + localMaxX;
        moveSpot = new GameObject("SpiderMoveSpot").transform;
        moveSpot.tag = "Enemies";
        moveSpot.position = new Vector2(Random.Range(minX, maxX), transform.position.y);
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!enemyAI.TargetInDistance())
        {
            anim.SetTrigger("Idle");
            RandomMovement();
        }
        else if(enemyAI.TargetInDistance())
        {
            anim.SetTrigger("Idle");
            Jump();
        }

        if(health.currentHealth <= 0)
        {
            FindObjectOfType<ItemsManager>().GetComponent<ItemsManager>().InstantiateRandomItems(null, droppingItems, transform.position, Quaternion.identity);
            Death();
        }

        if (groundCheck.isGrounded())
        {
            anim.ResetTrigger("Jump");
            anim.SetTrigger("Land");
        }

    }
    IEnumerator InstantiateAndPlaceOnGround(GameObject prefab, Vector3 position)
    {
        GameObject newObject = Instantiate(prefab, position, Quaternion.identity);
        // Try to ground the object
        yield return StartCoroutine(GroundObject(newObject));
    }

    IEnumerator GroundObject(GameObject obj)
    {
        GroundCheck groundCheck = obj.GetComponent<GroundCheck>();
        if (groundCheck != null)
        {
            float fallDistance = 0f;
            while (!groundCheck.isGrounded() && fallDistance < maxFallDistance)
            {
                obj.transform.position += Vector3.down * 0.1f;
                fallDistance += 0.1f;
                yield return new WaitForSeconds(checkInterval);
            }
        }
    }
    void Death()
    {
        //play death animation
        //Instantiate baby spiders
        anim.SetTrigger("Death");
        Instantiate(deathPoof, transform.position, Quaternion.identity);
        StartCoroutine(InstantiateAndPlaceOnGround(babySpiderPrefab, transform.position));
        
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
            enemyAI.isJumping = true;
            anim.SetTrigger("Jump");
            rb.velocity = new Vector2(rb.velocity.x, enemyAI.jumpForce);
            StartCoroutine(JumpCoolDown());
            //StartCoroutine(Grounded());
        }
    }

    IEnumerator JumpCoolDown()
    {
        isOnCoolDown = true;
        yield return new WaitForSeconds(2f);
        isOnCoolDown = false;
    }
}
