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
    private float dashCounter = 0f;
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
        dashCounter += Time.deltaTime;
    }
    void FixedUpdate()
    {
        if (isDashing && !CheckUI())
        {
            rb.velocity = dashDirection.normalized * dashVelocity;
        }
        if (!canDash && !CheckUI())
        {
            if (dashCounter >= dashCooldown && ground.isGrounded())
            {
                canDash = true;
                dashCounter = 0f;
            }
            else if(dashCounter >= dashCooldown && !ground.isGrounded())
            {
                canDash = false;
            }
        }
        
    }

    private bool CheckUI()
    {
        GameObject manager = GameObject.Find("Manager");
        return manager.GetComponent<InventoryManager>().CheckGeneralItemsUIOpeningState();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!knockback.isBeingKnockedBack)
        {
            if (context.started && canDash)
            {
                isDashing = true;
                GetComponent<PlayerAnimationController>().SetDashingState(true);
                AudioManager.instance.DashSound.Play();
                canDash = false;
                trail.emitting = true;
                dashDirection = new Vector2(movement.directionX, 0f);
                Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), true);
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
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), false);
        GetComponent<PlayerAnimationController>().SetDashingState(false);
        isDashing = false;
        trail.emitting = false;
    }
}
