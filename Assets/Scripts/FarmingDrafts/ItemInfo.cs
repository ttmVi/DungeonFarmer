using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    [SerializeField] private Items itemsData;

    public void SetItemData(Items item)
    {
        itemsData = item;
        GetComponent<SpriteRenderer>().sprite = itemsData.GetItemOverworldSprite();
    }

    public Items GetItemData()
    {
        return itemsData;
    }

    public void GetPickedUp(GameObject picker)
    {
        if (picker.TryGetComponent(out PlayerInventory playerInventory))
        {
            playerInventory.PickUpItems(itemsData, 1);
            Destroy(gameObject);
        }
    }
}
