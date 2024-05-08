using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private Items fullWaterBottle;
    [SerializeField] private Items emptyWaterBottle;

    public void EndFarmDay()
    {
        // This method will be called when the player ends the day
        // It will update the growth of all the trees in the farm

        // Get all the plots in the farm
        PlotFarming[] plots = FindObjectsOfType<PlotFarming>();
        foreach (PlotFarming plot in plots)
        {
            // Check if the plot has a tree planted
            if (plot.GetTreeData() != null)
            {
                // Update the growth of the tree
                plot.EndDayCheck();
            }
        }
    }

    public void FetchWater()
    {
        playerInventory.FetchWater(emptyWaterBottle, fullWaterBottle);
    }

    public void EmptyWater()
    {
        playerInventory.EmptyWaterBottle(fullWaterBottle, emptyWaterBottle);
    }

    public bool WaterBottleFilled()
    {
        return playerInventory.CheckForItem(fullWaterBottle);
    }
}
