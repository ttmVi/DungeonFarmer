using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLadderClimb : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    [SerializeField] private float climbingSpeed;
    [SerializeField] private LayerMask ladderLayer;
    [SerializeField] public bool isInLadder;
    public Vector2 climbingDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();

        climbingDirection = Vector2.zero;
    }

    private void Update()
    {
        if (isInLadder && LadderCheck())
        {
            transform.Translate(climbingDirection * climbingSpeed * Time.deltaTime);
        }
        else if (!LadderCheck() && GameObject.Find("Manager").GetComponent<GameManager>().inDungeon) { ExitLadder(); }
    }

    public void EnterLadder(InputAction.CallbackContext context)
    {
        if (LadderCheck() && context.ReadValue<Vector2>().y != 0)
        {
            if (!isInLadder)
            {
                rb.velocity = Vector3.zero;
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                GetComponent<PlayerMovement>().enabled = false;
                GetComponent<PlayerJump>().enabled = false;
                isInLadder = true;
                Debug.Log("Player is climbing ladder");
            }
            else { Debug.Log("Player has already climbed"); }
        }
        else { Debug.Log("No ladders found"); }
    }

    public void ExitLadder(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ExitLadder();
        }
    }

    private void ExitLadder()
    {
        if (isInLadder)
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            GetComponent<PlayerMovement>().enabled = true;
            GetComponent<PlayerJump>().enabled = true;
            isInLadder = false;
        }
    }

    public void OnClimbingLadder(InputAction.CallbackContext context)
    {
        if (isInLadder)
        {
            climbingDirection.y = context.ReadValue<Vector2>().y;
        }
    }

    public bool LadderCheck()
    {
        Collider2D ladder = Physics2D.OverlapBox((Vector2)transform.position + coll.offset, GetComponent<BoxCollider2D>().size - new Vector2(0f, 0.1f), 0f, ladderLayer);
        return ladder != null;
    }
}
