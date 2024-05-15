using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;

    [SerializeField] private Items fullWaterBottle;
    [SerializeField] private Items emptyWaterBottle;
    [SerializeField] private int waterBottleCapacity = 5;
    [SerializeField] private int currentWaterAmount;

    public void EndFarmDay()
    {
        // This method will be called when the player ends the day
        // It will update the growth of all the trees in the farm
        StartCoroutine(GoToSleep());
    }

    private IEnumerator GoToSleep()
    {
        GameManager manager = GetComponent<GameManager>();
        
        StartCoroutine(manager.BlackOut());
        yield return new WaitUntil(() => manager.ScreenIsBlack());

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
        yield return new WaitForSeconds(1f);

        StartCoroutine(manager.WhiteIn());
        yield return null;
    }

    public void FetchWater()
    {
        playerInventory.FetchWater(emptyWaterBottle, fullWaterBottle);
        currentWaterAmount = waterBottleCapacity;
    }

    public void EmptyWater()
    {
        if (currentWaterAmount > 0)
        {
            currentWaterAmount--;
        }

        if (currentWaterAmount <= 0) 
        { 
            playerInventory.EmptyWaterBottle(fullWaterBottle, emptyWaterBottle); 
        }
    }

    public bool WaterBottleFilled()
    {
        return playerInventory.CheckForItem(fullWaterBottle);
    }
}
