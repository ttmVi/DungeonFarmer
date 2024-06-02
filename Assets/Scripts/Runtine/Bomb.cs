using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D col;
    public GameObject explosion;
    public LayerMask layerMask;
    public int damageAmount = 40;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.deltaTime>5f)
        {
            Explode();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((layerMask.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            Explode();
        }
        if (collision.GetComponent<EnemyHealth>())
        {
            //Method that checks to see what force can be applied to the player when melee attacking
            HandleCollision(collision.GetComponent<EnemyHealth>());
        }
    }
    void Explode()
    {
        //Instantiate explosion
        Instantiate(explosion, transform.position, Quaternion.identity);
        //Destroy bomb
        Destroy(gameObject);
    }
    private void HandleCollision(EnemyHealth objHealth)
    {
        //Deals damage in the amount of damageAmount
        objHealth.Damage(damageAmount);
    }
}
