using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Canvas Elements Assigning")]
    [SerializeField] private GameObject generalItemsUI;
    [SerializeField] private GameObject inventoryCanvas;
    [SerializeField] private GameObject itemsList;
    [SerializeField] private GameObject selectButton;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private TextMeshProUGUI hints;
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerInventory playerInventory;

    [Header("Inventory UI Settings")]
    [SerializeField] private int numberOfRows;
    [SerializeField] private int numberOfColumns;
    [SerializeField] private int distanceBetweenSlots;

    [Header("Inventory States")]
    private bool UIOpened;
    [SerializeField] public bool isOpening;
    [SerializeField] private List<(Items, int)> displayingInventory;
    [Space]
    [SerializeField] private bool isPlantingTree;
    [SerializeField] private bool isFertilizingTree;
    [SerializeField] private GameObject plantingPlot;
    [SerializeField] private Inventory savedPlayerInventory;
    private bool lastButtonReleased;

    [Header("Current Inventory Indexes")]
    [SerializeField] private int startIndex;
    [SerializeField] private int selectingIndex;
    [SerializeField] private int currentInventoryIndex;

    private void Start()
    {
        startIndex = 0;
        selectingIndex = 0;
    }

    private void Update()
    {
        if (isOpening)
        {
            player.GetComponent<PlayerInteract>().enabled = false;
            player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponent<PlayerJump>().enabled = false;
            player.GetComponent<PlayerAttack>().enabled = false;
            player.GetComponent<PlayerDash>().enabled = false;
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        if (isOpening && displayingInventory != null)
        {
            DisplayInventory(displayingInventory);
            currentInventoryIndex = startIndex + selectingIndex;
        }

        UIOpened = CheckGeneralItemsUIOpeningState();
    }

    public bool CheckGeneralItemsUIOpeningState()
    {
        for (int i = 0; i < generalItemsUI.transform.childCount; i++)
        {
            if (generalItemsUI.transform.GetChild(i).gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    public void SavePlayerInventory()
    {
        savedPlayerInventory = playerInventory.playerInventory;
    }

    public void LoadPlayerInventory()
    {
        playerInventory.LoadInventory(savedPlayerInventory);
    }

    public void OnInventoryPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            lastButtonReleased = false;

            if (isOpening)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory(playerInventory.playerInventoryList);
            }
        }
        
        if (context.canceled) { lastButtonReleased = true; }
    }

    public void OpenSeedsInventory(GameObject plantingPlot)
    {
        OpenInventory(playerInventory.seedsInventory);
        isPlantingTree = true;
        this.plantingPlot = plantingPlot;
    }

    public void OpenFertilizersInventory(GameObject plantingPlot)
    {
        OpenInventory(playerInventory.fertilizersInventory);
        isFertilizingTree = true;
        this.plantingPlot = plantingPlot;
    }

    private void OpenInventory(List<(Items, int)> inventory)
    {
        if (!isOpening && !UIOpened && GetComponent<GameManager>().inFarm)
        {
            lastButtonReleased = false;

            displayingInventory = inventory;
            inventoryCanvas.SetActive(true);
            isOpening = true;

            if (displayingInventory == playerInventory.playerInventoryList)
            {
                hints.text = "These are all the items you have right now in your inventory";
            }
            else if (displayingInventory == playerInventory.seedsInventory)
            {
                hints.text = "You're looking at all the seeds you have, press x to plant your plant";
            }
            else if (displayingInventory == playerInventory.fertilizersInventory)
            {
                hints.text = "Looks like your water can is empty\n But it's okay, you can fertilize your plant instead";
            }
            else
            {
                hints.text = "";
            }

            player.GetComponent<PlayerInteract>().enabled = false;
            player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponent<PlayerJump>().enabled = false;
            player.GetComponent<PlayerAttack>().enabled = false;
            player.GetComponent<PlayerDash>().enabled = false;
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    public IEnumerator ClosingInventory()
    {
        yield return new WaitForFixedUpdate();

        inventoryCanvas.SetActive(false);
        isOpening = false;
        isPlantingTree = false;
        isFertilizingTree = false;
        plantingPlot = null;

        if (!CheckGeneralItemsUIOpeningState())
        {
            player.GetComponent<PlayerAnimationController>().ResetTriggerInteractingAnimation();
            player.GetComponent<PlayerInteract>().enabled = true;
            player.GetComponent<PlayerMovement>().enabled = true;
            player.GetComponent<PlayerDash>().enabled = true;
            player.GetComponent<PlayerJump>().enabled = true;
        }
    }

    public void CloseInventory()
    {
        StartCoroutine(ClosingInventory());
    }

    public void OnInventoryUINavigation(InputAction.CallbackContext context)
    {
        if (isOpening && selectButton.activeSelf)
        {
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

    public void OnUsingItem(InputAction.CallbackContext context)
    {
        if (context.started && lastButtonReleased)
        {
            lastButtonReleased = false;
            if (isPlantingTree && isOpening && displayingInventory.Count > 0)
            {
                Items plantingSeed = displayingInventory[currentInventoryIndex].Item1;
                plantingPlot.GetComponent<PlotFarming>().PlantSeed(plantingSeed.GetSeedData());
                playerInventory.gameObject.GetComponent<PlayerInventory>().RemoveItems(plantingSeed, 1);
                CloseInventory();
            }
            else if (isFertilizingTree && isOpening && displayingInventory.Count > 0)
            {
                Items fertilizer = displayingInventory[currentInventoryIndex].Item1;
                plantingPlot.GetComponent<PlotFarming>().FertilizePlant(fertilizer, 1.5f);
                //playerInventory.gameObject.GetComponent<PlayerInventory>().RemoveItems(fertilizer, 1);
                CloseInventory();
            }
        }
        else { lastButtonReleased = true; }
    }

    public void UseUpPotion(Items potion)
    {
        playerInventory.RemoveItems(potion, 1);
    }

    public bool CheckForItems(Items items)
    {
        return playerInventory.CheckForItem(items);
    }

    private void NextItem()
    {
        if (selectingIndex % numberOfColumns != numberOfColumns - 1)
        {
            if (!IsEmptySlot(selectingIndex + 1, displayingInventory))
            {
                selectingIndex++;
                selectButton.GetComponent<RectTransform>().anchoredPosition += new Vector2(distanceBetweenSlots, 0);
            }
        }
    }

    private void PreviousItem()
    {
        if (selectingIndex == 0) { return; }

        if (selectingIndex % numberOfColumns != 0)
        {
            if (!IsEmptySlot(selectingIndex - 1, displayingInventory))
            {
                selectingIndex--;
                selectButton.GetComponent<RectTransform>().anchoredPosition -= new Vector2(distanceBetweenSlots, 0);
            }
        }
        else { }
    }

    private void ForcePreviousItem()
    {
        if (selectingIndex == 0) { return; }

        if (selectingIndex % numberOfColumns != 0)
        {
            selectingIndex--;
            selectButton.GetComponent<RectTransform>().anchoredPosition -= new Vector2(distanceBetweenSlots, 0);
        }
        else
        {
            selectingIndex--;
            selectButton.GetComponent<RectTransform>().anchoredPosition += new Vector2(distanceBetweenSlots * (numberOfColumns - 1), 0);
            selectButton.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, distanceBetweenSlots);
        }
    }

    private void AboveItem()
    {
        if (selectingIndex == 0) { return; }

        if (!IsEmptySlot(selectingIndex - numberOfColumns, displayingInventory))
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
        if (!IsEmptySlot(selectingIndex + numberOfColumns, displayingInventory))
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

            restart:
            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    int slotIndex = i * numberOfColumns + j;
                    int realIndex = startIndex + slotIndex;
                    GameObject slot = itemsList.transform.GetChild(i).GetChild(j).gameObject;

                    if (realIndex >= inventory.Count)
                    {
                        DisplayItemSlot(slot, false);
                        if (selectingIndex == slotIndex)
                        {
                            Debug.Log("Selecting empty slot");
                            ForcePreviousItem();
                            goto restart;
                        }
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
            itemName.text = "";
            itemDescription.text = "";
        }
    }

    private void DisplaySelectingItem(GameObject slot, Items itemData, bool isSelecting)
    {
        if (isSelecting)
        {
            slot.GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 1.2f);
            itemName.text = itemData.GetItemName();
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
