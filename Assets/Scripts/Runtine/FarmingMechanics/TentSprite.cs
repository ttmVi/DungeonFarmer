using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentSprite : MonoBehaviour
{
    private bool isChangingSprite = false;
    private SpriteRenderer sprite;
    private Coroutine currentCoroutine;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(DisableSprite());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(EnableSprite());
        }
    }

    private IEnumerator DisableSprite()
    {
        float time = 1f;
        float elapsedTime = 0f;
        isChangingSprite = true;

        while (elapsedTime < time && isChangingSprite)
        {
            sprite.color = Color.Lerp(sprite.color, new Color(1, 1, 1, 0), elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        sprite.color = new Color(1, 1, 1, 0);
        isChangingSprite = false;
        currentCoroutine = null;
        yield return null;
    }

    private IEnumerator EnableSprite()
    {
        float time = 1f;
        float elapsedTime = 0f;
        isChangingSprite = true;

        while (elapsedTime < time && isChangingSprite)
        {
            sprite.color = Color.Lerp(sprite.color, new Color(1, 1, 1, 1), elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        sprite.color = new Color(1, 1, 1, 1);
        isChangingSprite = false;
        currentCoroutine = null;
        yield return null;
    }
}
