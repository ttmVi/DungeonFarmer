using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RisingMagma : MonoBehaviour
{
    [SerializeField] private float singleRisingTime;
    [SerializeField] private float tileWidth;
    [SerializeField] private GameObject animatedTiles;
    [SerializeField] private AnimatedTile sampleTile;

    private bool isRising;
    private Vector2 tileStartPosition;
    private Vector2 collStartPosition;
    private bool isNotReset = false;

    private void Start()
    {
        tileStartPosition = animatedTiles.transform.position;
        collStartPosition = transform.position;
        isNotReset = true;
    }

    void Update()
    {
        RiseUpMySauce();
    }

    public void ResetPosition()
    {
        if (isNotReset)
        {
            isRising = false;
            animatedTiles.transform.position = tileStartPosition;
            transform.position = collStartPosition;
        }
    }

    private void RiseUpMySauce()
    {
        transform.Translate(Vector2.up * (tileWidth / GetAnimationTime()) * Time.deltaTime);
        if (!isRising)
        {
            StartCoroutine(Riseee());
        }
    }

    private IEnumerator Riseee()
    {
        isRising = true;
        yield return new WaitForSeconds(GetAnimationTime() - Time.deltaTime * Time.deltaTime);
        isRising = false;
        animatedTiles.transform.position += new Vector3(0f, tileWidth, 0f);
    }

    private float GetAnimationTime()
    {
        return sampleTile.m_AnimatedSprites.Length / sampleTile.m_MinSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealth>().Die();
        }
    }
}