using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeyserEruption : MonoBehaviour
{
    [SerializeField] private float eruptionRate = 15f;
    [SerializeField] private float nextEruption = 0f;
    [SerializeField] private bool erupting = false;
    [SerializeField] private float eruptionDuration;

    private Animator animator;
    [SerializeField] private AnimationClip gasAnimation;
    [SerializeField] private AnimationClip eruptionAnimation;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        eruptionDuration = gasAnimation.length;
    }

    // Update is called once per frame
    void Update()
    {
        nextEruption += Time.deltaTime;

        if (nextEruption >= eruptionRate)
        {
            animator.SetTrigger("erupting");
            nextEruption = 0;
        }

        if (erupting)
        {
            StartCoroutine(Erupt());
        }
    }

    private IEnumerator Erupt()
    {
        erupting = false;

        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("releasingGas");

        yield return new WaitForSeconds(gasAnimation.length);
        FinishEruption();
    }

    private void FinishEruption()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
    }
}
