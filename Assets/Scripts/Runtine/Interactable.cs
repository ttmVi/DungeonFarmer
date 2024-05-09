using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    private PlayerInteract player;
    [SerializeField] private UnityEvent interactEvent;

    private Color originalColor = new Color(200f / 255f, 200f / 255f, 200f / 255f, 1f);

    // Start is called before the first frame update
    void Start()
    {
        restart:
        player = FindObjectOfType<PlayerInteract>();
        if (!player.gameObject.activeSelf) { goto restart; }
    }

    private void Update()
    {
        if (player.GetInteractingObject() == gameObject)
        {
            EnableInteractionSprite();
        }
        else { DisableInteractionSprite(); }
    }

    public void IsInteracted()
    {
        if (interactEvent != null)
        {
            interactEvent.Invoke();
        }
    }

    private void EnableInteractionSprite()
    {
        TryGetComponent(out SpriteRenderer sprite);
        sprite.color = Color.white;
    }

    private void DisableInteractionSprite()
    {
        TryGetComponent(out SpriteRenderer sprite);
        sprite.color = originalColor;
    }
}
