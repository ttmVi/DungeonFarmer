using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    [SerializeField] private Items itemsData;
    [SerializeField] private GameObject picker;

    private void Start()
    {
        if (itemsData != null) { SetItemData(itemsData); }
        SetPicker(GameObject.Find("Player"));
    }

    public void SetItemData(Items item)
    {
        itemsData = item;
        GetComponent<SpriteRenderer>().sprite = itemsData.GetItemOverworldSprite();

        if (itemsData.GetItemOverworldSprite() == null)
        {
            GetComponent<SpriteRenderer>().sprite = null;
        }
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
            GameManager manager = FindObjectOfType<GameManager>();
            if (manager.inFarm)
            {
                playerInventory.PickUpItems(gameObject.GetComponent<ItemInfo>().GetItemData(), 1);
                Destroy(gameObject);
            }
            else if (manager.inDungeon)
            {
                playerInventory.PickUpItemsInDungeon(gameObject.GetComponent<ItemInfo>().GetItemData(), 1);
                Destroy(gameObject);
            }
        }
    }
}
