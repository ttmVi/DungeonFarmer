using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ParallaxBackground : MonoBehaviour
{
    private float length;
    private Vector2 startPos;
    [SerializeField] private GameObject cam;
    [SerializeField] private float parallaxFactorX;
    [SerializeField] private float parallaxFactorY;

    // Start is called before the first frame update
    void Start()
    {
        length = GetComponent<TilemapRenderer>().bounds.size.x;
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float temp = (cam.transform.position.x * (1 - parallaxFactorX));

        float distanceX = cam.transform.position.x * parallaxFactorX;
        float distanceY = cam.transform.position.y * parallaxFactorY;

        transform.position = new Vector2(startPos.x + distanceX, startPos.y + distanceY);

        if (temp > startPos.x + length)
        {
            startPos += new Vector2(length, 0f);
        }
    }
}
