using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boar : MonoBehaviour
{
    public Vector2 startPosition;
    private Transform[] points;
    public float distanceFromStart;
    private int pointIndex;
    private EnemyAI enemyAI;
    private WallCheck wallCheck;
    private EnemyHealth health;
    //private Rigidbody2D rb;
    public float patrolSpeed = 5f;
    public float minX = 5f, maxX = 5f;
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
        points = new Transform[2];
        startPosition = transform.position;
        health = GetComponent<EnemyHealth>();
        enemyAI = GetComponent<EnemyAI>();
        wallCheck = GetComponent<WallCheck>();

        GameObject point1 = new GameObject();
        GameObject point2 = new GameObject();
        Transform point1Transform = point1.transform;
        Transform point2Transform = point2.transform;

        point1Transform.position = new Vector2(startPosition.x - minX, startPosition.y);
        point2Transform.position = new Vector2(startPosition.x + maxX, startPosition.y);

        points[0] = point1Transform;
        points[1] = point2Transform;
    }
    private void Update()
    {
        distanceFromStart = Vector2.Distance(transform.position, startPosition);
        if (!enemyAI.TargetInDistance())
        {
            anim.SetBool("PlayerNear", false);
            if (distanceFromStart > 10f)
            {
                transform.position = startPosition;
            }
            Patrol();
        }
        else
        {
            anim.SetBool("PlayerNear", true);
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
        anim.SetTrigger("Crash");
        enemyAI.followEnabled = false;
        yield return new WaitForSeconds(2f);
        anim.SetTrigger("Recover");
        yield return new WaitWhile(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);
        //yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);
        //yield return new WaitForSeconds(0.5f);
        anim.ResetTrigger("Crash");
        anim.ResetTrigger("Recover");
        enemyAI.followEnabled = true;
    }
}
