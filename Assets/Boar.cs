using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : MonoBehaviour
{
    public Transform playerLocation;
    private Rigidbody rb;
    public Transform[] points;
    public int pointIndex;
    public bool moving1;
    public bool moving2;
    private bool chasing;
    private bool patrol;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        patrol = true;
    }
    private void Update()
    {
        if (patrol)
        {
            Patrol();
        }
        else if(chasing)
        {
            Chasing();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            chasing = true;
            patrol = false;
        }
        
    }
    void Chasing()
    {
        if(transform.position != playerLocation.transform.position)
        {
            //transform.position = Vector2.MoveTowards(transform.position, location1.transform.position, 5f * Time.deltaTime);
            transform.position = Vector2.MoveTowards(transform.position, playerLocation.transform.position, 1f*Time.deltaTime);
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
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            patrol = true;
            chasing = false;
        }
    }
}
