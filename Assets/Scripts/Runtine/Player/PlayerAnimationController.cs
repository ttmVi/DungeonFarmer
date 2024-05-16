using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private GameManager gameManager;

    [SerializeField] private float lookAheadDistance = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("xSpeed", GetComponent<PlayerMovement>().directionX);
        animator.SetFloat("ySpeed", GetComponent<PlayerLadderClimb>().climbingDirection.y);

        SetMovingState(GetComponent<PlayerMovement>().directionX != 0);
        SetGroundingState(GetComponent<GroundCheck>().isGrounded());
        SetClimbingState(GetComponent<PlayerLadderClimb>().isInLadder);

        if (NearlyLanding()) { TriggerLandingAnimation(); }
    }

    public void TriggerWaterAnimation() { animator.SetTrigger("waterTree"); }
    public void TriggerInteractingAnimation() { animator.SetTrigger("interact"); }

    public void TriggerJumpingAnimation() 
    { 
        animator.SetTrigger("jumping");
        animator.ResetTrigger("landing");
    }
    public void TriggerLandingAnimation() 
    { 
        animator.SetTrigger("landing");
        animator.ResetTrigger("jumping");
    }

    public void SetClimbingState(bool state) { animator.SetBool("isClimbing", state); }

    public void SetGroundingState(bool state) { animator.SetBool("onGround", state); }

    public void SetMovingState(bool state) { animator.SetBool("isMoving", state); }

    private bool NearlyLanding()
    {
        bool nearGround = false;
        BoxCollider2D coll = GetComponent<BoxCollider2D>();
        Collider2D ground = Physics2D.OverlapBox((Vector2)transform.position + coll.offset + new Vector2(0f, lookAheadDistance), new Vector2(coll.size.x, 0.1f), 0f, LayerMask.GetMask("Ground"));
        nearGround = Physics2D.Raycast((Vector2)transform.position + coll.offset, Vector2.down, coll.size.y / 2 + lookAheadDistance, LayerMask.GetMask("Ground"));
        return ground != null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.GetMask("Ground"))
        {
            animator.ResetTrigger("jumping");
        }
    }
}
