using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    private Knockback knockback;
    private PlayerMovement movement;
    private Rigidbody2D rb;
    private GroundCheck ground;
    private TrailRenderer trail;
    private bool canDash = true;
    private bool isDashing = false;
    private Vector2 dashDirection;
    [Header("Dashing Stats")]
    [SerializeField] private float dashVelocity = 14f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCooldown = 0.6f;
    // Start is called before the first frame update
    void Start()
    {
        knockback = GetComponent<Knockback>();
        movement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        ground = GetComponent<GroundCheck>();
        trail = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!canDash)
        {
            dashCooldown -= Time.deltaTime;
            if(dashCooldown <= 0 && ground.isGrounded()) //old version doesnt check for isGrounded, feels better but can dash in air after falling for 0.6 seconds
            {
                canDash = true;
                dashCooldown = 0.6f;
            }
        }
    }
    void FixedUpdate()
    {
        if (isDashing)
        {
            rb.velocity = dashDirection.normalized * dashVelocity;
            return;
        }

        if (ground.isGrounded())
        {
            canDash = true;
        }
    }
    public void OnDash(InputAction.CallbackContext context)
    {
        if (!knockback.isBeingKnockedBack)
        {
            if (context.started && canDash)
            {
                isDashing = true;
                canDash = false;
                trail.emitting = true;
                dashDirection = new Vector2(movement.directionX, 0f);  
                if(dashDirection == Vector2.zero)
                {
                    dashDirection = new Vector2(transform.localScale.x,0f);
                }
                StartCoroutine(StopDash());
            }
        }
    }

    private IEnumerator StopDash()
    {
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        trail.emitting = false;
    }
}
