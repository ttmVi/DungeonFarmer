using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mosquito : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform target;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.5f;
    public float distanceToTarget;
    public Vector3 startPosition;

    [Header("Physics")]
    public float speed = 3f;
    public float dashMultiplier = 10f;
    public float nextWaypointDistance = 3f;

    [Header("Custom Behavior")]
    public bool followEnabled = true;
    public bool directionLookEnabled = true;
    public bool coroutineRunning = false;

    [Header("Circular Movement")]
    public float rotationRadius = 2f;
    public float angularSpeed = 2f;
    float posX, posY, angle = 0f;

    [Header("Random Movement")]
    public float waitTime;
    public float startWaitTime;
    public float minX, minY, maxX, maxY;
    public Transform moveSpot;
    public float flitterSpeed = 1f;

    [SerializeField] Vector3 startOffset;

    private Path path;
    private int currentWaypoint = 0;
    Seeker seeker;
    Rigidbody2D rb;

    public void Start()
    {
        waitTime = startWaitTime;
        minX = transform.position.x - 2f;
        maxX = transform.position.x + 2f;
        minY = transform.position.y - 2f;
        maxY = transform.position.y + 2f;
        moveSpot = new GameObject("MoveSpot").transform;
        //moveSpot = transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        moveSpot.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        startPosition = rb.position;
        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    private void FixedUpdate()
    {
        distanceToTarget = Vector2.Distance(transform.position, target.position);
        if(distanceToTarget <= 5f && !coroutineRunning)
        {
            StartCoroutine(Attack());
        }
        //If target is in distance, follow it, otherwise flitter
        if (TargetInDistance() && followEnabled)
        {
            PathFollow();
        }
        
    }
    private void Update()
    {
        if (!coroutineRunning && !TargetInDistance())
        {
            //Flitter();
            RandomMovement();
        }
    }
    void Flitter()
    {
        posX = startPosition.x + Mathf.Cos(angle) * rotationRadius;
        posY = startPosition.y + Mathf.Sin(angle) * rotationRadius;
        transform.position = new Vector2(posX, posY);
        angle = angle + Time.deltaTime * angularSpeed;
        if (angle >= 360f)
        {
            angle = 0f;
        }
    }
    void RandomMovement()
    {
        transform.position = Vector2.MoveTowards(transform.position, moveSpot.position, flitterSpeed * Time.deltaTime);
        
        if (Vector2.Distance(transform.position,moveSpot.position)<0.2f)
        {
            if(waitTime<=0)
            {
                moveSpot.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
                while (Vector2.Distance(transform.position,moveSpot.position)<1.5f)
                {
                    moveSpot.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
                }
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
    IEnumerator Attack()
    {
        coroutineRunning = true;
        followEnabled = false;
        yield return new WaitForSeconds(1f);
        //calculate direction to dash
        //Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 direction = ((Vector2)target.position - rb.position).normalized;
        Vector2 force = direction * speed * dashMultiplier;
        //Dash towards player
        rb.velocity = new Vector2(force.x, force.y);
        yield return new WaitForSeconds(1f);
        //move back to position in the sky
        StartCoroutine(Return());
        
    }
    IEnumerator Return()
    {
        while(rb.position.y != startPosition.y)
        {
            Vector2 direction = ((Vector2)startPosition - rb.position).normalized;
            Vector2 force = direction * speed;
            rb.velocity = new Vector2(rb.velocity.x, force.y);
            rb.MovePosition(rb.position + force * Time.fixedDeltaTime);
            if (rb.position.y+0.1f> startPosition.y)
            {
                break;
            }
            else
            {
                yield return null;
            }
        }
        followEnabled = true;
        coroutineRunning = false;
    }
    private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void PathFollow()
    {
        if (path == null)
        {
            return;
        }

        // Reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        // See if colliding with anything
        startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y, transform.position.z);

        // Direction Calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed;

        

        // Movement
        rb.velocity = new Vector2(force.x, rb.velocity.y);
        //rb.AddForce(force);

        // Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Direction Graphics Handling
        if (directionLookEnabled)
        {
            if (rb.velocity.x > 0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (rb.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    public bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}
