using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Melee : MonoBehaviour
{
    //How much damage the melee attack does
    [SerializeField]
    private int damageAmount = 20;
    //Reference to Character script which contains the value if the player is facing left or right; if you don't have this or it's named something different, either omit it or change the class name to what your Character script is called
    //private GroundCheck character;
    //Reference to the Rigidbody2D on the player
    private Rigidbody2D rb;
    //Reference to the MeleeAttackManager script on the player
    private PlayerAttack meleeAttackManager;
    //Reference to the direction the player needs to go in after melee weapon contacts something
    private Vector2 direction;
    //Bool that manages if the player should move after melee weapon colides
    private bool collided;
    //Determines if the melee strike is downwards to perform extra force to fight against gravity
    public bool downwardStrike;
    public bool resetGravity = false; //Bool that manages if the player should reset gravity after a downward strike
    public GroundCheck groundCheck;
    public GameObject player;
    //public GameObject character;
    private void Start()
    {
        //Reference to the Character script on the player; if you don't have this or it's named something different, either omit it or change the class name to what your Character script is called
        //character = GetComponent<GroundCheck>();
        //Debug.Log("character: " + character.name);
        //Reference to the Rigidbody2D on the player
        rb = GetComponentInParent<Rigidbody2D>();
        //Reference to the MeleeAttackManager script on the player
        meleeAttackManager = GetComponentInParent<PlayerAttack>();
        //Debug.Log("meleeAttackManager: " + meleeAttackManager.name);
    }

    private void FixedUpdate()
    {
        //Uses the Rigidbody2D AddForce method to move the player in the correct direction
        //HandleMovement(); //Weaker version, harder to stay in the air
        StartCoroutine(DelayedHandleMovement());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Checks to see if the GameObject the MeleeWeapon is colliding with has an EnemyHealth script
        if (collision.GetComponent<EnemyHealth>())
        {
            //Method that checks to see what force can be applied to the player when melee attacking
            HandleCollision(collision.GetComponent<EnemyHealth>());
        }
    }

    private void HandleCollision(EnemyHealth objHealth)
    {
        //Checks to see if the GameObject allows for upward force and if the strike is downward as well as grounded
        if (objHealth.giveUpwardForce && player.GetComponent<PlayerMovement>().directionY < 0 && !player.GetComponent<GroundCheck>().isGrounded())
        {
            //Sets the direction variable to up
            direction = Vector2.up;
            //Sets downwardStrike to true
            downwardStrike = true;
            resetGravity = true;
            StartCoroutine(WaitToTurnOffResetGravity());
            //Sets collided to true
            collided = true;
        }
        if (player.GetComponent<PlayerMovement>().directionY > 0 && !player.GetComponent<GroundCheck>().isGrounded())
        {
            //Sets the direction variable to up
            direction = Vector2.down;
            //Sets collided to true
            collided = true;
        }
        //Checks to see if the melee attack is a standard melee attack
        if ((player.GetComponent<PlayerMovement>().directionY <= 0 && player.GetComponent<GroundCheck>().isGrounded()) || player.GetComponent<PlayerMovement>().directionY == 0)
        {
            //Checks to see if the player is facing left; if you don't have a character script, the commented out line of code can also check for that
            if (transform.parent.localScale.x < 0) //(character.isFacingLeft)
            {
                //Sets the direction variable to right
                direction = Vector2.right;
            }
            else
            {
                //Sets the direction variable to right left
                direction = Vector2.left;
            }
            //Sets collided to true
            collided = true;
        }
        //Deals damage in the amount of damageAmount
        objHealth.Damage(damageAmount);
        //Coroutine that turns off all the bools related to melee attack collision and direction
        StartCoroutine(NoLongerColliding());
    }

    //Method that makes sure there should be movement from a melee attack and applies force in the appropriate direction based on the amount of force from the melee attack manager script
    private void HandleMovement()
    {
        //Checks to see if the GameObject should allow the player to move when melee attack colides
        if (collided)
        {
            //If the attack was in a downward direction
            if (downwardStrike)
            {
                //Propels the player upwards by the amount of upwardsForce in the meleeAttackManager script
                rb.velocity = Vector2.zero;
                rb.AddForce(direction * meleeAttackManager.upwardsForce);
            }
            else
            {
                //Propels the player backwards by the amount of horizontalForce in the meleeAttackManager script
                rb.AddForce(direction * meleeAttackManager.defaultForce);
            }
        }
    }

    //Coroutine that turns off all the bools that allow movement from the HandleMovement method
    private IEnumerator NoLongerColliding()
    {
        //Waits in the amount of time setup by the meleeAttackManager script; this is by default .1 seconds
        yield return new WaitForSeconds(meleeAttackManager.movementTime);
        //Turns off the collided bool
        collided = false;
        //Turns off the downwardStrike bool
        downwardStrike = false;
    }
    private IEnumerator DelayedHandleMovement()
    {
        if (collided)
        {
            //If the attack was in a downward direction
            if (downwardStrike)
            {
                //Propels the player upwards by the amount of upwardsForce in the meleeAttackManager script

                //yield return new WaitForSeconds(0.05f);
                //yield return new WaitForEndOfFrame();
                rb.velocity = Vector2.zero;
                //yield return new WaitForEndOfFrame();
                rb.AddForce(direction * meleeAttackManager.upwardsForce, ForceMode2D.Impulse);
                yield return new WaitForEndOfFrame();
            }
            else
            {
                //Propels the player backwards by the amount of horizontalForce in the meleeAttackManager script
                rb.AddForce(direction * meleeAttackManager.defaultForce, ForceMode2D.Impulse);
            }
        }
    }
    private IEnumerator WaitToTurnOffResetGravity()
    {
        while (!groundCheck.isGrounded())
        {
            yield return null;
        }
        if(groundCheck.isGrounded())
        {
            resetGravity = false;
        }   
    }
}
