using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : MonoBehaviour
{
    public Transform playerLocation;
    private Rigidbody rb;
    public Transform location1;
    public Transform location2;
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
            transform.position = Vector2.Lerp(transform.position, playerLocation.transform.position, 1f*Time.deltaTime);
        }
    }
    void Patrol()
    {
        if(transform.position.x != location1.transform.position.x)
        {
            moving1 = true;
        }
        else if (transform.position.x != location1.transform.position.x)
        {
            moving2 = true;
        }




        if (moving1)
        {
            transform.position = Vector2.MoveTowards(transform.position, location1.transform.position, 1f*Time.deltaTime);
            moving2 = false;
        }
        else if(moving2)
        {
            transform.position = Vector2.MoveTowards(transform.position, location2.transform.position, 1f * Time.deltaTime);
            moving1 = false;
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
