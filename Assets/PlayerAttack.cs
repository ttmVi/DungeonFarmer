using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class PlayerAttack : MonoBehaviour
{
    //How much the player should move either downwards or horizontally when melee attack collides with a GameObject that has EnemyHealth script on it
    public float defaultForce = 300;
    //How much the player should move upwards when melee attack collides with a GameObject that has EnemyHealth script on it
    public float upwardsForce = 300;
    //How long the player should move when melee attack collides with a GameObject that has EnemyHealth script on it
    public float movementTime = .1f;
    //Input detection to see if the button to perform a melee attack has been pressed
    
    //The animator on the meleePrefab
    private Animator meleeAnimator;

    private bool desiredAttack;
    private bool pressingAttack;
    private float attackTimeCounter;
    private bool currentlyAttacking;
    private int attackBuffer;
    private float attackBufferCounter;
    private bool canAttackAgain;
    

    public float attackDelay = 1f;
    public float attackDelayCounter;

    //public GameObject player;
    //public TextMeshProUGUI 
    //The Animator component on the player
    private Animator anim;
    //The Character script on the player; this script on my project manages the grounded state, so if you have a different script for that reference that script
    private GroundCheck character;
    private PlayerMovement movement;
    private Melee melee;
    private Knockback knockback;
    //Run this method instead of Initialization if you don't have any scripts inheriting from each other
    private void Start()
    {
        //The Animator component on the player
        //anim = GetComponent<Animator>();
        //The Character script on the player; this script on my project manages the grounded state, so if you have a different script for that reference that script
        character = GetComponent<GroundCheck>();
        //Debug.Log("character: "+character.name);
        //The animator on the meleePrefab
        meleeAnimator = GetComponentInChildren<Melee>().gameObject.GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        melee = GetComponentInChildren<Melee>();
        knockback = GetComponent<Knockback>();

        //Debug.Log("meleeAnimator: " + meleeAnimator.name);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            desiredAttack = true;
            //pressingAttack = true;
            //CheckInput();
            //meleeAnimator.SetTrigger("ForwardMeleeSwipe");
            
        }
    }

    private void CheckInput()
    {
        //Checks to see if meleeAttack is true and pressing up
        if (movement.directionY > 0)
        {
            //Turns on the animation for the player to perform an upward melee attack
            //anim.SetTrigger("UpwardMelee");
            //Turns on the animation on the melee weapon to show the swipe area for the melee attack upwards
            meleeAnimator.SetTrigger("UpwardMeleeSwipe");
        }
        //Checks to see if meleeAttack is true and pressing down while also not grounded
        if (movement.directionY < 0 && !character.isGrounded())
        {
            //Turns on the animation for the player to perform a downward melee attack
            //anim.SetTrigger("DownwardMelee");
            //Turns on the animation on the melee weapon to show the swipe area for the melee attack downwards
            meleeAnimator.SetTrigger("DownwardMeleeSwipe");
        }
        //Checks to see if meleeAttack is true and not pressing any direction OR if melee attack is true and pressing down while grounded
        if (( movement.directionY == 0) || (movement.directionY < 0 && character.isGrounded()))
        {
            //Turns on the animation for the player to perform a forward melee attack
            //anim.SetTrigger("ForwardMelee");
            //Turns on the animation on the melee weapon to show the swipe area for the melee attack forwards
            meleeAnimator.SetTrigger("ForwardMeleeSwipe");
        }
    }

    private void Update()
    {
        //Attack buffer allows us to queue up an attack, which will play when the attack animation is finished
        if (attackBuffer > 0)
        {
            //Instead of immediately turning off "desireAttack", start counting up...
            //All the while, the DoAttack function will repeatedly be fired off
            if (desiredAttack)
            {
                attackBufferCounter += Time.deltaTime;

                if (attackBufferCounter > attackBuffer)
                {
                    //If time exceeds the jump buffer, turn off "desireJump"
                    desiredAttack = false;
                    attackBufferCounter = 0;
                }
            }
        }
        if (!currentlyAttacking)
        {
            attackTimeCounter += Time.deltaTime;
        }
        else
        {
            attackTimeCounter = 0;
        }
        attackDelayCounter += Time.deltaTime;
    }
    private void FixedUpdate()
    {
        if (!knockback.isBeingKnockedBack)
        {
            //Keep trying to do a attack, for as long as desiredAttack is true
            if (desiredAttack)
            {
                DoAttack();
                return;
            }
        }
    }
    private void DoAttack()
    {
        //Create the attack, provided we have a attack available
        if (attackDelayCounter>attackDelay)
        {
            desiredAttack = false;
            attackBufferCounter = 0;
            attackTimeCounter = 0;
            CheckInput();
            currentlyAttacking = true;
            attackDelayCounter = 0;
        }

        if (attackBuffer == 0)
        {
            //If we don't have a attack buffer, then turn off desiredAttack immediately after hitting attack
            desiredAttack = false;
        }
    }
}
