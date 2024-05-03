using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Canvas Elements Assigning")]
    [SerializeField] private GameObject inventoryCanvas;
    [SerializeField] private GameObject itemsList;
    [SerializeField] private GameObject selectButton;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private PlayerInventory playerInventory;

    [Header("Inventory UI Settings")]
    [SerializeField] private int numberOfRows;
    [SerializeField] private int numberOfColumns;
    [SerializeField] private int distanceBetweenSlots;

    [Header("Inventory States")]
    [SerializeField] public bool isOpening;

    [Header("Current Inventory Indexes")]
    [SerializeField] private int startIndex;
    [SerializeField] private int selectingIndex;

    private void Start()
    {
        startIndex = 0;
        selectingIndex = 0;
    }

    private void Update()
    {
        if (isOpening)
        {
            DisplayInventory(playerInventory.playerInventoryList);
        }
    }

    public void OnInventoryPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (isOpening)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }
    }

    private void OpenInventory()
    {
        inventoryCanvas.SetActive(true);
        isOpening = true;
    }

    private void CloseInventory()
    {
        inventoryCanvas.SetActive(false);
        isOpening = false;
    }

    public void OnInventoryUINavigation(InputAction.CallbackContext context)
    {
        if (isOpening && selectButton.activeSelf)
        {
            Debug.Log("Inputting");
            switch (context.ReadValue<Vector2>().x)
            {
                case -1: PreviousItem(); break;
                case 1: NextItem(); break;
                default: break;
            }
            switch (context.ReadValue<Vector2>().y)
            {
                case -1: BelowItem(); break;
                case 1: AboveItem(); break;
                default: break;
            }
        }
        else { Debug.Log("Inventory not opened / Inventory empty"); }
    }

    private bool IsEmptySlot(int rowIndex, int columnIndex, List<(Items, int)> currentInventory)
    {
        return startIndex + (rowIndex * numberOfColumns + columnIndex) >= currentInventory.Count;
    }

    private bool IsEmptySlot(int slotIndex, List<(Items, int)> currentInventory)
    {
        return startIndex + slotIndex >= currentInventory.Count;
    }

    private void NextItem()
    {
        if (selectingIndex % numberOfColumns != numberOfColumns - 1)
        {
            if (!IsEmptySlot(selectingIndex + 1, playerInventory.playerInventoryList))
            {
                selectingIndex++;
                selectButton.GetComponent<RectTransform>().anchoredPosition += new Vector2(distanceBetweenSlots, 0);
            }
        }
    }

    private void PreviousItem()
    {
        if (selectingIndex % numberOfColumns != 0)
        {
            if (!IsEmptySlot(selectingIndex - 1, playerInventory.playerInventoryList))
            {
                selectingIndex--;
                selectButton.GetComponent<RectTransform>().anchoredPosition -= new Vector2(distanceBetweenSlots, 0);
            }
        }
        else { }
    }

    private void AboveItem()
    {
        if (!IsEmptySlot(selectingIndex - numberOfColumns, playerInventory.playerInventoryList))
        {
            if (selectingIndex / numberOfColumns < 1)
            {
                ScrollUp();
            }
            else
            {
                selectingIndex -= numberOfColumns;
                selectButton.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, distanceBetweenSlots);
            }
        }
    }

    private void BelowItem()
    {
        if (!IsEmptySlot(selectingIndex + numberOfColumns, playerInventory.playerInventoryList))
        {
            if (selectingIndex / numberOfColumns >= numberOfRows - 1)
            {
                ScrollDown();
            }
            else
            {
                selectingIndex += numberOfColumns;
                selectButton.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, distanceBetweenSlots);
            }
        }
    }

    private void ScrollDown()
    {
        startIndex += numberOfColumns;
    }

    private void ScrollUp()
    {
        if (startIndex > 0)
        {
            startIndex -= numberOfColumns;
        }
        else { startIndex = 0; }
    }

    public void DisplayInventory(List<(Items, int)> inventory)
    {
        if (inventory.Count != 0)
        {
            itemsList.SetActive(true);
            selectButton.SetActive(true);

            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    int slotIndex = i * numberOfRows + j;
                    int realIndex = startIndex + slotIndex;
                    GameObject slot = itemsList.transform.GetChild(i).GetChild(j).gameObject;

                    if (realIndex >= inventory.Count)
                    {
                        DisplayItemSlot(slot, false);
                    }
                    else
                    {
                        DisplayItemSlot(slot, inventory[realIndex].Item1.GetItemIcon(), inventory[realIndex].Item2, true);

                        if (slotIndex == selectingIndex)
                        {
                            DisplaySelectingItem(slot, inventory[realIndex].Item1, true);
                        }
                        else
                        {
                            DisplaySelectingItem(slot, inventory[realIndex].Item1, false);
                        }
                    }
                }
            }
        }
        else 
        { 
            itemsList.SetActive(false); 
            selectButton.SetActive(false);
            itemDescription.text = "";
        }
    }

    private void DisplaySelectingItem(GameObject slot, Items itemData, bool isSelecting)
    {
        if (isSelecting)
        {
            slot.GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 1.2f);
            itemDescription.text = itemData.GetItemDescription();
        }
        else
        {
            slot.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
        }
    }
    
    private void DisplayItemSlot(GameObject slot, Sprite itemIcon, int quantity, bool isActive)
    {
        slot.SetActive(isActive);
        slot.GetComponent<Image>().sprite = itemIcon;
        slot.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "x" + quantity.ToString();
    }

    private void DisplayItemSlot(GameObject slot, bool isActive)
    {
        slot.SetActive(isActive);
    }
}
