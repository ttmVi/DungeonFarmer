using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
    }
    void Update()
    {

    }
    public void DodgeInteract(InputAction.CallbackContext context)
    {
        //Code for dodge and interact
    }
    public void Move(InputAction.CallbackContext context)
    {
        rb.velocity = context.ReadValue<Vector2>()*5;
    }
    public void Attack(InputAction.CallbackContext context)
    {
        //Code for Attack
    }
    public void UseItem(InputAction.CallbackContext context)
    {
        //Code for using an item
    }
    public void Inventory(InputAction.CallbackContext context)
    {
        //Code for opening inventory
    }
}
