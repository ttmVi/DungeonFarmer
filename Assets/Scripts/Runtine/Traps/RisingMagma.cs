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

    // Update is called once per frame
    void Update()
    {
        RiseUpMySauce();
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
}