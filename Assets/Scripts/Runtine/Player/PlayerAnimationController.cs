using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private GameManager gameManager;

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
    }

    public void TriggerWaterAnimation() { animator.SetTrigger("waterTree"); }
    public void TriggerInteractingAnimation() { animator.SetTrigger("interact"); }

    public void TriggerJumpingAnimation() { animator.SetTrigger("jumping"); }
    public void TriggerLandingAnimation() { animator.SetTrigger("landing"); }

    public void SetClimbingState(bool state) { animator.SetBool("isClimbing", state); }

    public void SetGroundingState(bool state) { animator.SetBool("onGround", state); }

    public void SetMovingState(bool state) { animator.SetBool("isMoving", state); }
}
