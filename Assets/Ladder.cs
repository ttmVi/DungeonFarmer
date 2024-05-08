using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Ladder : MonoBehaviour
{
    private Rigidbody2D body;
    GroundCheck ground;
    private PlayerMovement playerMovement;
    private float previousGravity;
    public bool climbingLadder = false;
    public bool ladderCollision = false;
    private float speed = 5f;
    private void Awake()
    {
        
        playerMovement = GetComponent<PlayerMovement>();
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<GroundCheck>();
        previousGravity = body.gravityScale;
    }
    private void Update()
    {
        if (ladderCollision && Mathf.Abs(playerMovement.directionY) > 0f)
        {
            climbingLadder = true;
        }
    }
    private void FixedUpdate()
    {
        if (climbingLadder)
        {
            previousGravity = body.gravityScale;
            body.gravityScale = 0f;
            body.velocity = new Vector2(body.velocity.x, playerMovement.directionY*speed);
        }
        else
        {
            
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            ladderCollision = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            ladderCollision = false;
            climbingLadder = false;
            body.gravityScale = previousGravity;
        }
    }
}
