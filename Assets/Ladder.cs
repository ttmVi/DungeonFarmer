using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private Rigidbody2D body;
    GroundCheck ground;
    private float previousGravity;
    public bool climbingLadder = false;
    public bool ladderCollision = false;
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<GroundCheck>();
    }
    private void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            Debug.Log("Entered Ladder");
            body.velocity = new Vector2(0f, 0f);
            ladderCollision = true;
            previousGravity = body.gravityScale;
            body.gravityScale = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            ladderCollision = false;
            Debug.Log("Exit Ladder");
            body.gravityScale = previousGravity;
        }
    }
}
