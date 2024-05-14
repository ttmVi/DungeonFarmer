using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    //private Rigidbody2D rb;
    private float speed;
    //public Transform target;
    private Vector2 direction;
    private EnemyHealth health;
    //private Vector2 force;
    //public bool lookingLeft;

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
        health = GetComponent<EnemyHealth>();
    }
    
    public void Initialize(Vector2 direction,float speed)
    {
        this.direction = direction.normalized;
        this.speed = speed;
    }
    // Update is called once per frame
    void Update()
    {
        if(health.currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
        transform.Translate(direction * speed * Time.deltaTime);
        if(Time.deltaTime > 10f)
        {
            Destroy(this.gameObject);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Wall") || collision.CompareTag("Ground"))
        {
            Destroy(this.gameObject);
        }
    }
}
