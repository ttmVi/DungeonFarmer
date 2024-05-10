using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ItemsManager;

[RequireComponent(typeof(Interactable))]
public class PlotFarming : MonoBehaviour
{
    [Header("Plot Sprites")]
    [SerializeField] private Sprite unploughedPlot;
    [SerializeField] private Sprite ploughedPlot;
    [SerializeField] private Sprite seededPlot;

    [Header("Debris Possible Items")]
    [SerializeField] private Items[] theItems;

    [Space]
    [Header("Tree Information")]
    [SerializeField] private Items draftingTree;
    [SerializeField] private GameObject treePlot;
    private Tree treeData;
    [SerializeField] private string currentTree;
    [SerializeField] private float treeGrowthIndex;
    [SerializeField] private float maximumStackedWater;
    [SerializeField] private float stackedWater;
    [SerializeField] private float maximumStackedFertilizer;
    [SerializeField] private float stackedFertilizer;
    private int currentTreePhase;
    private float maxGrowthIndex;

    [Header("Item Drop")]
    [SerializeField] private GameObject itemPlaceholder;

    private void ResetTreePlotData()
    {
        //treePlot.GetComponent<SpriteRenderer>().sprite = null;
        treeData = null;
        currentTree = null;
        treeGrowthIndex = -1;
        currentTreePhase = 0;
        maxGrowthIndex = 0;
        stackedWater = 0;
        stackedFertilizer = 0;
    }

    // This method is called when the player interacts with the plot
    public void DoFarming()
    {
        // Ploughing the plot (or cleaning the debris)
        if (GetComponent<SpriteRenderer>().sprite == unploughedPlot) 
        {
            CleanDebris();
            PloughPlot();
        }

        // Planting the seed
        else if (GetComponent<SpriteRenderer>().sprite == ploughedPlot) 
        {
            FindObjectOfType<InventoryManager>().GetComponent<InventoryManager>().OpenSeedsInventory(gameObject);
        }
        //else if (GetComponent<SpriteRenderer>().sprite == ploughedPlot) { PlantSeed(draftingTree.GetTreeData()); }

        // Watering and fertilizing the plant
        else if (GetComponent<SpriteRenderer>().sprite != ploughedPlot && GetComponent<SpriteRenderer>().sprite != unploughedPlot)
        {
            if (treeGrowthIndex < maxGrowthIndex && treeData != null)
            {
                if (FindObjectOfType<FarmManager>().GetComponent<FarmManager>().WaterBottleFilled())
                {
                    WaterPlant(0.5f);
                }
                else
                {
                    FindObjectOfType<InventoryManager>().GetComponent<InventoryManager>().OpenFertilizersInventory(gameObject);
                }

                //WaterPlant(0.2f);
                //FertilizePlant(0.1f);
            }
            else if (treeGrowthIndex >= maxGrowthIndex && treeData != null)
            {
                HarvestPlant();
            }
        }
    }

    private void CleanDebris()
    {
        foreach (Items item in theItems)
        {
            //itemsManager.InstantiateItem(itemPlaceholder, item, transform.position, Quaternion.identity);
            itemsManager.InstantiateItemInLine(itemPlaceholder, item, transform.position, 0.1f, 1f, Quaternion.identity);
        }
    }

    private void PloughPlot()
    {
        GetComponent<SpriteRenderer>().sprite = ploughedPlot;
    }

    public void PlantSeed(Tree seed)
    {
        GetComponent<SpriteRenderer>().sprite = seed.growingPhasesSprites[1];
        //treePlot.GetComponent<SpriteRenderer>().sprite = seed.growingPhasesSprites[1];
        treeGrowthIndex = 0;

        // Getting tree information
        treeData = seed;
        currentTree = seed.treeName;
        currentTreePhase = 1;
        maxGrowthIndex = seed.maxGrowthIndex;
    }

    private void WaterPlant(float wateringAmount)
    {
        FarmManager manager = FindObjectOfType<FarmManager>().GetComponent<FarmManager>();
        
        if (!manager.WaterBottleFilled())
        {
            Debug.Log("No water in the bottle");
            return;
        }
        else
        {
            manager.EmptyWater();

            stackedWater += wateringAmount;
            if (stackedWater >= maximumStackedWater)
            {
                stackedWater = maximumStackedWater;
            }
        }
    }

    public void FertilizePlant(Items fertilizer, float fertilizingAmount)
    {
        if (fertilizer.fertilizableSeed.GetItemName() != currentTree)
        {
            Debug.Log("Wrong type of fertilizer");
            FindObjectOfType<InventoryManager>().GetComponent<InventoryManager>().OpenFertilizersInventory(gameObject);
            return;
        }
        else
        {
            stackedFertilizer += fertilizingAmount;
            if (stackedFertilizer >= maximumStackedFertilizer)
            {
                stackedFertilizer = maximumStackedFertilizer;
            }

            FindObjectOfType<PlayerInventory>().GetComponent<PlayerInventory>().RemoveItems(fertilizer, 1);
        }
    }

    private void HarvestPlant()
    {
        // Drop plant's items
        for (int i = 0; i < treeData.possibleDrops.Length; i++)
        {
            //itemsManager.InstantiateItem(itemPlaceholder, treeData.possibleDrops[i], transform.position, Quaternion.identity);
            itemsManager.InstantiateItemInLine(itemPlaceholder, treeData.possibleDrops[i], transform.position, 0.1f, 1.5f, Quaternion.identity);
        }

        RemovePlant();
    }

    public void EndDayCheck()
    {
        if (treeData != null)
        {
            if (stackedFertilizer <= 0 && stackedWater <= 0)
            {
                if (treeData.growingPhasesSprites.Contains(GetComponent<SpriteRenderer>().sprite))
                {
                    GetComponent<SpriteRenderer>().sprite = treeData.deceasingSprites[currentTreePhase];
                }
                else { RemovePlant(); }
            }
            else
            {
                if (treeData.deceasingSprites.Contains(GetComponent<SpriteRenderer>().sprite))
                {
                    RevitalizePlant();
                }
                else
                {
                    GrowPlant();
                    CheckTreeGrowth();
                }
            }
        }
    }

    private void GrowPlant()
    {
        treeGrowthIndex += stackedWater + stackedFertilizer;
        if (treeGrowthIndex >= maxGrowthIndex)
        {
            treeGrowthIndex = maxGrowthIndex;
        }

        stackedWater = 0;
        stackedFertilizer = 0;
    }

    private void RevitalizePlant()
    {
        GetComponent<SpriteRenderer>().sprite = treeData.growingPhasesSprites[currentTreePhase];

        stackedWater = 0;
        stackedFertilizer = 0;
    }

    private void RemovePlant()
    {
        ResetTreePlotData();
        GetComponent<SpriteRenderer>().sprite = ploughedPlot;
    }

    private void CheckTreeGrowth()
    {
        // Changing tree sprite based on growth index
        if (treeData != null)
        {
            for (int i = 0; i < treeData.phasesGrowthIndex.Length; i++)
            {
                if (treeGrowthIndex < treeData.phasesGrowthIndex[i])
                {
                    GetComponent<SpriteRenderer>().sprite = treeData.growingPhasesSprites[i - 1];
                    currentTreePhase = i - 1;
                    break;
                }
                else if (i == treeData.phasesGrowthIndex.Length - 1)
                {
                    GetComponent<SpriteRenderer>().sprite = treeData.growingPhasesSprites[i];
                    currentTreePhase = i;
                    break;
                }
                else 
                {
                    continue; 
                }
            }
        }
    }

    public Tree GetTreeData() { return treeData; }
}
