using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    [SerializeField] private Items itemsData;
    [SerializeField] private GameObject picker;

    public void SetItemData(Items item)
    {
        itemsData = item;
        GetComponent<SpriteRenderer>().sprite = itemsData.GetItemOverworldSprite();
    }

    public void SetPicker(GameObject picker)
    {
        this.picker = picker;
    }

    public Items GetItemData()
    {
        return itemsData;
    }

    public void GetPickedUp()
    {
        if (picker.TryGetComponent(out PlayerInventory playerInventory))
        {
            playerInventory.PickUpItems(gameObject.GetComponent<ItemInfo>().GetItemData(), 1);
            Destroy(gameObject);
        }
    }
}
