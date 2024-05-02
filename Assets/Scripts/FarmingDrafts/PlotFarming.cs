using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class PlotFarming : MonoBehaviour
{
    [Header("Plot Sprites")]
    [SerializeField] private Sprite unploughedPlot;
    [SerializeField] private Sprite ploughedPlot;
    [SerializeField] private Sprite seededPlot;

    [Header("Debris Possible Items")]
    [SerializeField] private Items[] theItems;

    [Header("Tree Information")]
    [SerializeField] private Tree draftingTree;
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
        treePlot.GetComponent<SpriteRenderer>().sprite = null;
        treeData = null;
        currentTree = null;
        treeGrowthIndex = -1;
        currentTreePhase = 0;
        maxGrowthIndex = 0;
        stackedWater = 0;
        stackedFertilizer = 0;
    }

    private void Update()
    {

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
        else if (GetComponent<SpriteRenderer>().sprite == ploughedPlot) { PlantSeed(draftingTree); }

        // Watering and fertilizing the plant
        else if (GetComponent<SpriteRenderer>().sprite == seededPlot)
        {
            if (treeGrowthIndex < maxGrowthIndex && treeData != null)
            {
                WaterPlant(0.2f);
                FertilizePlant(0.1f);
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
            GameObject debris = Instantiate(itemPlaceholder, transform.position, Quaternion.identity);
            debris.GetComponent<ItemInfo>().SetItemData(item);
        }
    }

    private void PloughPlot()
    {
        GetComponent<SpriteRenderer>().sprite = ploughedPlot;
    }

    private void PlantSeed(Tree seed)
    {
        GetComponent<SpriteRenderer>().sprite = seededPlot;
        treePlot.GetComponent<SpriteRenderer>().sprite = seed.growingPhasesSprites[1];
        treeGrowthIndex = 0;

        // Getting tree information
        treeData = seed;
        currentTree = seed.GetItemName();
        currentTreePhase = 1;
        maxGrowthIndex = seed.maxGrowthIndex;
    }

    private void WaterPlant(float wateringAmount)
    {
        stackedWater += wateringAmount;
        if (stackedWater >= maximumStackedWater)
        {
            stackedWater = maximumStackedWater;
        }
    }

    private void FertilizeSoil() { }

    private void FertilizePlant(float fertilizingAmount)
    {
        stackedFertilizer += fertilizingAmount;
        if (stackedFertilizer >= maximumStackedFertilizer)
        {
            stackedFertilizer = maximumStackedFertilizer;
        }
    }

    private void HarvestPlant()
    {
        // Drop plant's items
        for (int i = 0; i < treeData.possibleDrops.Length; i++)
        {
            GameObject item = Instantiate(itemPlaceholder, transform.position, Quaternion.identity);
            item.GetComponent<ItemInfo>().SetItemData(treeData.possibleDrops[i]);
        }

        RemovePlant();
    }

    public void EndDayCheck()
    {
        if (treeData != null)
        {
            if (stackedFertilizer <= 0 && stackedWater <= 0)
            {
                if (treeData.growingPhasesSprites.Contains(treePlot.GetComponent<SpriteRenderer>().sprite))
                {
                    treePlot.GetComponent<SpriteRenderer>().sprite = treeData.deceasingSprites[currentTreePhase];
                    Debug.Log(currentTreePhase);
                }
                else { RemovePlant(); }
            }
            else
            {
                if (treeData.deceasingSprites.Contains(treePlot.GetComponent<SpriteRenderer>().sprite))
                {
                    RevitalizePlant();
                    Debug.Log(currentTreePhase);
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
        treePlot.GetComponent<SpriteRenderer>().sprite = treeData.growingPhasesSprites[currentTreePhase];

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
                    treePlot.GetComponent<SpriteRenderer>().sprite = treeData.growingPhasesSprites[i - 1];
                    currentTreePhase = i - 1;
                    Debug.Log(currentTreePhase);
                    break;
                }
                else if (i == treeData.phasesGrowthIndex.Length - 1)
                {
                    treePlot.GetComponent<SpriteRenderer>().sprite = treeData.growingPhasesSprites[i];
                    currentTreePhase = i;
                    Debug.Log(currentTreePhase);
                    break;
                }
                else 
                {
                    Debug.Log("Plant's phase is not " + i);
                    continue; 
                }
            }
        }
    }

    public Tree GetTreeData() { return treeData; }
}
