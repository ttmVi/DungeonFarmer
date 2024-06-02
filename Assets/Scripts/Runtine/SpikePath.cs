using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikePath : MonoBehaviour
{
    public Transform[] points;
    public float speed = 5f;
    private int pointIndex;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = points[pointIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(pointIndex < points.Length)
        {
            transform.position = Vector2.MoveTowards(transform.position, points[pointIndex].transform.position, speed * Time.deltaTime);
            if(transform.position == points[pointIndex].transform.position)
            {
                pointIndex+=1;
            }
            if(pointIndex == points.Length)
            {
                pointIndex = 0;
            }

        }
    }
}
