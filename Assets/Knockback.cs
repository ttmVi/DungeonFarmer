using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float knockbackTime = 0.2f;
    public float hitDirectionForce = 10f;
    public float constForce = 5f;
    public float inputForce = 7.5f;

    private Rigidbody2D rb;
    private Coroutine knockbackCoroutine;
    public bool isBeingKnockedBack { get; private set; }

    IEnumerator KnockbackAction(Vector2 hitDirection, Vector2 constantForceDirection, float inputDirection)
    {
        isBeingKnockedBack = true;

        Vector2 hitForce;
        Vector2 constantForce;
        Vector2 knockbackForce;
        Vector2 combinedForce;

        hitForce = hitDirection * hitDirectionForce;
        constantForce = constantForceDirection * constForce;

        float elapsedTime = 0;
        while(elapsedTime< knockbackTime)
        {
            elapsedTime += Time.deltaTime;
            knockbackForce = hitForce + constantForce;
            if(inputDirection != 0f)
            {
                combinedForce = knockbackForce + new Vector2(inputDirection*inputForce, 0f);
            }
            else
            {
                combinedForce = knockbackForce;
            }
            //apply knockback force to the rigidbody
            rb.velocity = Vector3.zero;
            //rb.AddForce(combinedForce, ForceMode2D.Impulse);
            rb.velocity = combinedForce;
            yield return new WaitForFixedUpdate();
        }
        isBeingKnockedBack = false;
    }
    public void callKnockBack(Vector2 hitDirection, Vector2 constantForceDirection, float inputDirection)
    {
        /*if(knockbackCoroutine != null)
        {
            StopCoroutine(knockbackCoroutine);
        }*/
        knockbackCoroutine = StartCoroutine(KnockbackAction(hitDirection, constantForceDirection, inputDirection));
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
