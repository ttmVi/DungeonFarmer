using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGenerator : MonoBehaviour
{
    [SerializeField] private GameObject rockPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnRate = 2f;
    [SerializeField] private float nextSpawn = 0f;
    [SerializeField] private bool isVomitting = false;
    private bool justVomitted = false;

    private Animator animator;
    [SerializeField] private AnimationClip vomitAnimation;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spawnPoint = transform;
    }

    // Update is called once per frame
    void Update()
    {
        nextSpawn += Time.deltaTime;
        if (nextSpawn >= spawnRate)
        {
            animator.SetTrigger("vomitting");
            nextSpawn = 0;
            justVomitted = false;
        }

        if (isVomitting && !justVomitted)
        {
            justVomitted = true;
            isVomitting = false;
            Vomit();
        }
    }

    public void Vomit()
    {
        isVomitting = false;
        Instantiate(rockPrefab, spawnPoint.position, Quaternion.identity);
        isVomitting = false;
    }
}
