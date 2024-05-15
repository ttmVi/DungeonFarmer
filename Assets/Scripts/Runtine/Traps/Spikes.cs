using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private float cooldownTime = 2f;
    [SerializeField] private float coolingTime;

    private Animator animator;
    [SerializeField] private AnimationClip spikeOut;
    [SerializeField] private AnimationClip spikeIn;

    private bool isSpiking;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        cooldownTime += spikeIn.length;
        coolingTime = cooldownTime;
    }

    // Update is called once per frame
    void Update()
    {
        coolingTime -= Time.deltaTime;

        if (PlayerNearRange() && !IsCoolingDown() &&!isSpiking)
        {
            StartCoroutine(Spike());
        }
        else { GetComponent<KnockbackTrigger>().enabled = false; }
    }

    private void RestartCooldown()
    {
        coolingTime = cooldownTime;
    }

    private bool IsCoolingDown() { return coolingTime > 0; }

    private bool PlayerDetected()
    {
        Collider2D coll = Physics2D.OverlapBox(transform.position, GetComponent<BoxCollider2D>().size, 0, LayerMask.GetMask("Player"));
        return coll != null;
    }

    private bool PlayerNearRange()
    {
        Collider2D coll = Physics2D.OverlapBox(transform.position, GetComponent<BoxCollider2D>().size + new Vector2(1f, 1f), 0, LayerMask.GetMask("Player"));
        return coll != null;
    }

    private GameObject GetPlayer()
    {
        Collider2D coll = Physics2D.OverlapBox(transform.position, GetComponent<BoxCollider2D>().size, 0, LayerMask.GetMask("Player"));
        return coll.gameObject;
    }

    private IEnumerator Spike()
    {
        isSpiking = true;
        animator.SetTrigger("playerDetected");
        yield return new WaitForSeconds(spikeOut.length );
        if (PlayerDetected())
        {
            //GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<KnockbackTrigger>().enabled = true;
            //GetPlayer().GetComponent<PlayerHealth>().TakeDamage(1, Vector2.up);
            RestartCooldown();
        }
        isSpiking = false;
    }
}
