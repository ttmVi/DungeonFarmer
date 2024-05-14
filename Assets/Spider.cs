using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    private EnemyHealth health;
    private EnemyAI enemyAI;
    private Vector2 startPosition;
    
    [Header("Random Movement")]
    public float waitTime;
    public float startWaitTime;
    [SerializeField, Range(0f, 2f)][Tooltip("Sets range of random movement based on local position")] public float localMinX, localMaxX;
    private float minX, maxX;
    private Transform moveSpot;
    public float patrolSpeed = 5f;
    void Start()
    {
        startPosition = transform.position;
        health = GetComponent<EnemyHealth>();
        enemyAI = GetComponent<EnemyAI>();

        waitTime = startWaitTime;
        minX = transform.position.x - localMinX;
        maxX = transform.position.x + localMaxX;
        moveSpot = new GameObject("SpiderMoveSpot").transform;
        moveSpot.position = new Vector2(Random.Range(minX, maxX), transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (!enemyAI.TargetInDistance())
        {
            RandomMovement();
        }
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
}
