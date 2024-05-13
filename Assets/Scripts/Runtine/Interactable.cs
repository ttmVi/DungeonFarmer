using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    protected PlayerInteract player;
    [SerializeField][Tooltip("Interacting priority should only be positive integer")] private int interactingPriority = 0;
    [Space]
    [SerializeField] private UnityEvent interactEvent;

    protected Color originalColor = new Color(200f / 255f, 200f / 255f, 200f / 255f, 1f);

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
        
        if (TryGetComponent(out PlotFarming farmPlot))
        {
            //Debug.Log(farmPlot.gameObject.name);
            farmPlot.DoFarming();
        }
    }

    public int GetInteractingPriority() { return interactingPriority; }

    protected virtual void EnableInteractionSprite()
    {
        TryGetComponent(out PlotFarming plotFarming);
        TryGetComponent(out SpriteRenderer sprite);

        if (plotFarming == null)
        {
            sprite.color = Color.white;
        }
        else
        {
            if (plotFarming.GetComponent<SpriteRenderer>().sprite == plotFarming.ploughedPlot)
            {
                sprite.color = originalColor;
            }
            else
            {
                sprite.color = Color.white;
            }
        }
    }

    protected virtual void DisableInteractionSprite()
    {
        TryGetComponent(out PlotFarming plotFarming);
        TryGetComponent(out SpriteRenderer sprite);

        if (plotFarming == null)
        {
            sprite.color = originalColor;
        }
        else
        {
            if (plotFarming.GetComponent<SpriteRenderer>().sprite == plotFarming.ploughedPlot)
            {
                sprite.color = Color.white;
            }
            else
            {
                sprite.color = originalColor;
            }
        }
    }
}
