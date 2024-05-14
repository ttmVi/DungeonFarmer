using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : MonoBehaviour
{
    public Vector2 startPosition;
    public Transform[] points;
    public float distanceFromStart;
    public int pointIndex;
    private EnemyAI enemyAI;
    private WallCheck wallCheck;
    private EnemyHealth health;
    //private Rigidbody2D rb;
    public float patrolSpeed = 5f;
    private void Awake()
    {
        //rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        health = GetComponent<EnemyHealth>();
        enemyAI = GetComponent<EnemyAI>();
        wallCheck = GetComponent<WallCheck>();
    }
    private void Update()
    {
        distanceFromStart = Vector2.Distance(transform.position, startPosition);
        if (!enemyAI.TargetInDistance())
        {
            if(distanceFromStart > 10f)
            {
                transform.position = startPosition;
            }
            Patrol();
        }

        if (wallCheck.isWall())
        {
            StartCoroutine(Stun());
        }

        if(health.currentHealth <= 0)
        {
            //play death animation
            Destroy(gameObject);
        }
    }
    void Patrol()
    {
        if (pointIndex < points.Length)
        {
            transform.position = Vector2.MoveTowards(transform.position, points[pointIndex].transform.position, patrolSpeed * Time.deltaTime);
            if (transform.position.x < points[pointIndex].transform.position.x)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (transform.position.x > points[pointIndex].transform.position.x)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }

            if (transform.position == points[pointIndex].transform.position)
            {
                pointIndex += 1;
            }
            if (pointIndex == points.Length)
            {
                pointIndex = 0;
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Stun());
        }
    }

    IEnumerator Stun()
    {
        //play stun animation
        enemyAI.followEnabled = false;
        yield return new WaitForSeconds(2f);
        enemyAI.followEnabled = true;
    }
}
