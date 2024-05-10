using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableFarmPlot : Interactable
{
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

    protected override void EnableInteractionSprite()
    {
        TryGetComponent(out SpriteRenderer sprite);
        TryGetComponent(out PlotFarming plotFarming);
        
        if (plotFarming.GetComponent<SpriteRenderer>().sprite == plotFarming.ploughedPlot)
        {
            sprite.color = originalColor;
        }
        else { base.EnableInteractionSprite(); }
    }

    protected override void DisableInteractionSprite()
    {
        TryGetComponent(out SpriteRenderer sprite);
        TryGetComponent(out PlotFarming plotFarming);

        if (plotFarming.GetComponent<SpriteRenderer>().sprite == plotFarming.ploughedPlot)
        {
            sprite.color = Color.white;
        }
        else { base.EnableInteractionSprite(); }
    }
}
