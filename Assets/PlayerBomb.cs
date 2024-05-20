using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBomb : MonoBehaviour
{
    private Knockback knockback;
    private PlayerInventory inventory;
    public Items bomb;
    public GameObject bombPrefab;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        knockback = GetComponent<Knockback>();
        inventory = GetComponent<PlayerInventory>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnBomb(InputAction.CallbackContext context)
    {
        if (!knockback.isBeingKnockedBack)
        {
            if (context.started)
            {
                if (inventory.CheckForItem(bomb))
                {
                    inventory.RemoveItems(bomb,1);
                    //Throw Bomb
                    ThrowBomb();
                    //GameObject bomb = Instantiate(bomb, transform.position, Quaternion.identity);
                }
            }
        }
    }

    void ThrowBomb()
    {
        //instantiate bombprefab in an arc
        Vector3 bombPos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        GameObject pr = Instantiate(bombPrefab, bombPos, Quaternion.identity);
        pr.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * (5 + Mathf.Clamp(Mathf.Abs(rb.velocity.x), 0, 3)), 5);
    }
}
