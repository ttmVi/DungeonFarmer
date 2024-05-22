using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraEndingWall : MonoBehaviour
{
    private GameObject theBoss;

    void Awake()
    {
        theBoss = GameObject.Find("SnakeBoss");
    }

    // Update is called once per frame
    void Update()
    {
        if (theBoss == null)
        {
            Destroy(gameObject);
        }
    }
}
