using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CraftTableManager : MonoBehaviour
{
    [Header("Crafting Canvas Elements Assigning")]
    [SerializeField] private GameObject craftingCanvas;
    [SerializeField] private GameObject recipesList;
    [SerializeField] private GameObject selectButton;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private float distanceBetweenRows;
    [SerializeField] private float distanceBetweenColumns;

    [Header("Items To Craft")]
    [SerializeField] private Items[] fertilizers;
    [SerializeField] private Items[] potions;

    [Header("Crafting Table Status")]
    [SerializeField] private bool isRightmost;
    [SerializeField] private bool isLeftmost;
    [SerializeField] private bool isUpmost;
    [SerializeField] private bool isDownmost;
    public bool isCrafting;
    private Items[] currentDisplayingRecipes;

    private void Start() { currentDisplayingRecipes = fertilizers; }

    private void Update()
    {
        if (isCrafting)
        {
            DisplayRecipes(currentDisplayingRecipes);
        }
    }

    public void OpenCraftingUI()
    {
        craftingCanvas.SetActive(true);
        isCrafting = true;
    }

    public void CloseCraftingUI(InputAction.CallbackContext context)
    {
        if (craftingCanvas.activeSelf && context.started)
        {
            craftingCanvas.SetActive(false);
            isCrafting = false;
        }
    }

    public void CloseCraftingUI()
    {
        if (craftingCanvas.activeSelf)
        {
            craftingCanvas.SetActive(false);
            isCrafting = false;
        }
    }

    public void OnCraftingUINavigation(InputAction.CallbackContext context)
    {
        if (isCrafting)
        {
            switch (context.ReadValue<Vector2>().x)
            {
                case -1: if (!isLeftmost)
                    {
                        PreviousRecipe();
                    }
                break;
                case 1: if (!isRightmost)
                    {
                        NextRecipe();
                    }
                break;
                default: break;
            }
            switch (context.ReadValue<Vector2>().y)
            {
                case -1: if (!isDownmost)
                    {
                        BelowRecipe();
                    }
                break;
                case 1: if (!isUpmost)
                    {
                        AboveRecipe();
                    }
                break;
                default: break;
            }
        }
    }

    private void PreviousRecipe()
    {
        selectButton.GetComponent<RectTransform>().anchoredPosition -= new Vector2(distanceBetweenColumns, 0f);
        isLeftmost = true;
        isRightmost = false;
    }

    private void NextRecipe()
    {
        selectButton.GetComponent<RectTransform>().anchoredPosition += new Vector2(distanceBetweenColumns, 0f);
        isRightmost = true;
        isLeftmost = false;
    }

    private void AboveRecipe()
    {
        selectButton.GetComponent<RectTransform>().anchoredPosition += new Vector2(0f, distanceBetweenRows);
        isUpmost = true;
        isDownmost = false;
    }
    private void BelowRecipe()
    {
        selectButton.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0f, distanceBetweenRows);
        isDownmost = true;
        isUpmost = false;
    }

    public void OnCraftingItem(InputAction.CallbackContext context)
    {
        if (isCrafting && context.started)
        {
            if (isLeftmost)
            {
                if (isUpmost) { Craft(currentDisplayingRecipes[0]); }
                else if (isDownmost) { Craft(currentDisplayingRecipes[2]); }
            }
            else if (isRightmost)
            {
                if (isUpmost) { Craft(currentDisplayingRecipes[1]); }
                else if (isDownmost) { Craft(currentDisplayingRecipes[3]); }
            }
        }
    }

    private Items[] GetItemCraftingRecipe(Items item)
    {
        if (item.GetItemType() == Items.ItemType.Fertilizer)
        {
            return item.GetFertilizerData().GetCraftingRecipe();
        }
        else if (item.GetItemType() == Items.ItemType.Potion)
        {
            return item.GetPotionData().GetCraftingRecipe();
        }
        return null;
    }

    private void DisplayRecipes(Items[] categorizedItems)
    {
        for (int i = 0; i < recipesList.transform.childCount; i++)
        {
            Items[] recipe = categorizedItems[i].GetCraftingRecipe();
            TextMeshProUGUI itemName = recipesList.transform.GetChild(i).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI ingredient1Name = recipesList.transform.GetChild(i).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI ingredient1Required = recipesList.transform.GetChild(i).GetChild(1).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI ingredient2Name = recipesList.transform.GetChild(i).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI ingredient2Required = recipesList.transform.GetChild(i).GetChild(2).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            Transform craftButton = recipesList.transform.GetChild(i).GetChild(3);


            itemName.text = categorizedItems[i].GetItemName();
            ingredient1Name.text = recipe[0].GetItemName();
            ingredient2Name.text = recipe[1].GetItemName();
            ingredient1Required.text = playerInventory.GetItemQuantity(recipe[0]).ToString() + " / 1";
            ingredient2Required.text = playerInventory.GetItemQuantity(recipe[1]).ToString() + " / 1";

            if (HasEnoughIngredients(categorizedItems[i]))
            {
                craftButton.GetComponent<Image>().color = Color.white;
            }
            else
            {
                craftButton.GetComponent<Image>().color = Color.gray;
            }
        }
    }

    private bool HasEnoughIngredients(Items item)
    {
        Items[] recipe = GetItemCraftingRecipe(item);

        foreach (Items ingredient in recipe)
        {
            if (!playerInventory.CheckForItem(ingredient))
            {
                return false;
            }
            else { continue; }
        }

        return true;
    }

    public void Craft(Items item)
    {
        if (HasEnoughIngredients(item))
        {
            Items[] recipe = GetItemCraftingRecipe(item);

            foreach (Items ingredient in recipe)
            {
                playerInventory.RemoveItems(ingredient, 1);
            }

            playerInventory.PickUpItems(item, 1);
            CloseCraftingUI();
        }
        else { Debug.Log("Not enough materials"); }
    }
}
