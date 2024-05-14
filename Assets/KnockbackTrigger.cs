using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackTrigger : MonoBehaviour
{
    [Header("Knockback options")]
    public int damage = 1;
    public float changeTime = 0.05f;
    public int restoreSpeed = 10;
    public float delay = 0.4f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if(transform.localScale.x > 0)
            {
                collision.gameObject.GetComponent<TimeStop>().StopTime(changeTime, restoreSpeed, delay);
                collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage, -transform.right);
            }
            else
            {
                collision.gameObject.GetComponent<TimeStop>().StopTime(changeTime, restoreSpeed, delay);
                collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage, transform.right);
            }
            
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (transform.localScale.x > 0)
            {
                collision.gameObject.GetComponent<TimeStop>().StopTime(changeTime, restoreSpeed, delay);
                collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage, -transform.right);
            }
            else
            {
                collision.gameObject.GetComponent<TimeStop>().StopTime(changeTime, restoreSpeed, delay);
                collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage, transform.right);
            }

        }
    }
}
