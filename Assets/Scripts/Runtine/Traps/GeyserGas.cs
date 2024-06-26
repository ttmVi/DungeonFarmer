using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeyserGas : MonoBehaviour
{
    private SpriteRenderer sprite;
    [SerializeField] private float damage = 2f;
    [SerializeField] private bool isDamagingPlayer = true;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sprite.enabled && GetPlayer() != null)
        {
            isDamagingPlayer = true;

            GetPlayer().GetComponent<PlayerHealth>().TakeDamage(Time.deltaTime * damage);
        }
        else { isDamagingPlayer = false; }
    }

    private GameObject GetPlayer()
    {
        Collider2D coll = Physics2D.OverlapCircle(transform.position, GetComponent<CircleCollider2D>().radius, LayerMask.GetMask("Player"));
        if (coll != null)
        {
            return coll.gameObject;
        }
        return null;
    }
}
