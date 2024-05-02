using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Canvas Elements Assigning")]
    [SerializeField] private GameObject inventoryCanvas;
    [SerializeField] private GameObject itemsList;
    [SerializeField] private PlayerInventory playerInventory;

    [Header("Inventory UI Settings")]
    [SerializeField] private int numberOfRows;
    [SerializeField] private int numberOfColumns;

    private void Update()
    {
        DisplayInventory(playerInventory.playerInventoryList);
    }

    public void DisplayInventory(List<(Items, int)> inventory)
    {
        if (inventory.Count != 0)
        {
            itemsList.SetActive(true);
            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    int index = i * numberOfRows + j;
                    GameObject slot = itemsList.transform.GetChild(i).GetChild(j).gameObject;

                    if (index >= inventory.Count)
                    {
                        slot.GetComponent<Image>().sprite = null;
                    }
                    else
                    {
                        slot.GetComponent<Image>().sprite = inventory[index].Item1.GetItemIcon();
                    }
                }
            }
        }
        else { itemsList.SetActive(false); }
    }
}
