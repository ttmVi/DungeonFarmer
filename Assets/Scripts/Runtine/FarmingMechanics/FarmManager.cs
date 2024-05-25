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
    [SerializeField] private AudioClip waterFetchingSound;

    public void EndFarmDay()
    {
        // This method will be called when the player ends the day
        // It will update the growth of all the trees in the farm
        StartCoroutine(GoToSleep());
    }

    public void ForcedEndFarmDay()
    {
        // This method will be called after player coming back from the dungeon
        // It will not call the black out and white in effect
        CheckFarmPlots();
    }

    private IEnumerator GoToSleep()
    {
        GameManager manager = GetComponent<GameManager>();
        
        StartCoroutine(manager.BlackOut());
        yield return new WaitUntil(() => manager.ScreenIsBlack());

        CheckFarmPlots();
        yield return new WaitForSeconds(1f);

        StartCoroutine(manager.WhiteIn());
        yield return null;
    }

    private void CheckFarmPlots()
    {
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
        if (playerInventory.CheckForItem(emptyWaterBottle))
        {
            playerInventory.FetchWater(emptyWaterBottle, fullWaterBottle);
            currentWaterAmount = waterBottleCapacity;
            AudioSource.PlayClipAtPoint(waterFetchingSound, playerInventory.gameObject.transform.position);
        }
        else if (playerInventory.CheckForItem(fullWaterBottle) && currentWaterAmount < waterBottleCapacity)
        {
            //playerInventory.FetchWater(emptyWaterBottle, fullWaterBottle);
            currentWaterAmount = waterBottleCapacity;
            AudioSource.PlayClipAtPoint(waterFetchingSound, playerInventory.gameObject.transform.position);
        }
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
