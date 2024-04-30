using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlotFarming : MonoBehaviour
{
    [Header("Plot Sprites")]
    [SerializeField] private Sprite unploughedPlot;
    [SerializeField] private Sprite ploughedPlot;
    [SerializeField] private Sprite seededPlot;

    [Header("Tree Information")]
    [SerializeField] private Tree draftingTree;
    [SerializeField] private GameObject treePlot;
    private Tree treeData;
    [SerializeField] private string currentTree;
    [SerializeField] private float treeGrowthIndex;
    [SerializeField] private float stackedWater;
    [SerializeField] private float stackedFertilizer;
    private int currentTreePhase;
    private float maxGrowthIndex;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndDayCheck();
        }
    }

    public void DoFarming()
    {
        if (GetComponent<SpriteRenderer>().sprite == unploughedPlot) { PloughPlot(); }
        else if (GetComponent<SpriteRenderer>().sprite == ploughedPlot) { PlantSeed(draftingTree); }
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

    private void PloughPlot()
    {
        GetComponent<SpriteRenderer>().sprite = ploughedPlot;
    }

    private void PlantSeed()
    {
        GetComponent<SpriteRenderer>().sprite = seededPlot;
    }

    private void PlantSeed(Tree seed)
    {
        GetComponent<SpriteRenderer>().sprite = seededPlot;
        treePlot.GetComponent<SpriteRenderer>().sprite = seed.growingPhasesSprites[1];
        treeGrowthIndex = 0;

        // Getting tree information
        treeData = seed;
        currentTree = seed.GetItemName();
        maxGrowthIndex = seed.maxGrowthIndex;
    }

    private void WaterPlant(float wateringAmount)
    {
        stackedWater += wateringAmount;
    }

    private void FertilizeSoil() { }

    private void FertilizePlant(float fertilizingAmount)
    {
        stackedFertilizer += fertilizingAmount;
    }

    private void HarvestPlant()
    {
        // Drop plant's items
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
                    treePlot.GetComponent<SpriteRenderer>().sprite = treeData.growingPhasesSprites[currentTreePhase];
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

    private void RemovePlant()
    {
        treeGrowthIndex = -1;
        treeData = null;
        treePlot.GetComponent<SpriteRenderer>().sprite = null;
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
                }
                else if (i == treeData.phasesGrowthIndex.Length - 1)
                {
                    treePlot.GetComponent<SpriteRenderer>().sprite = treeData.growingPhasesSprites[i];
                    currentTreePhase = i;
                }
                else { continue; }
            }
            Debug.Log(currentTreePhase);
        }
    }
}
