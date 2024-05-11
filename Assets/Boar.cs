using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : MonoBehaviour
{
    public Transform[] points;
    public int pointIndex;
    public bool moving1;
    public bool moving2;
    private EnemyAI enemyAI;
    private WallCheck wallCheck;
    private void Awake()
    {
        enemyAI = GetComponent<EnemyAI>();
        wallCheck = GetComponent<WallCheck>();
    }
    private void Update()
    {
        if (!enemyAI.TargetInDistance())
        {
            Patrol();
        }

        if (wallCheck.isWall())
        {
            StartCoroutine(Stun());
        }
    }
    void Patrol()
    {
        if (pointIndex < points.Length)
        {
            transform.position = Vector2.MoveTowards(transform.position, points[pointIndex].transform.position, 5f * Time.deltaTime);
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
