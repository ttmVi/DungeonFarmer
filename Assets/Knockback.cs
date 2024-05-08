using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D player = collision.gameObject.GetComponent<Rigidbody2D>();
            player.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
            collision.gameObject.GetComponent<TimeStop>().StopTime(0.05f, 10, 0.1f);
        }
    }
}
