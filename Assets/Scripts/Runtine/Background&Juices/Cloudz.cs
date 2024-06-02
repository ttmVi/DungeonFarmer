using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Cloudz : MonoBehaviour
{
    [SerializeField] private float movingSpeed;
    private float minXBound;
    private float maxXBound;

    private void Start()
    {
        minXBound = GetComponent<TilemapRenderer>().bounds.min.x;
        maxXBound = GetComponent<TilemapRenderer>().bounds.max.x;
    }

    private void Update()
    {
        transform.Translate(Vector2.left * movingSpeed * Time.deltaTime, Space.World);
        if (GetComponent<TilemapRenderer>().bounds.max.x < minXBound)
        {
            transform.position += new Vector3(GetComponent<TilemapRenderer>().bounds.size.x * 2, 0f);
        }
    }
}
