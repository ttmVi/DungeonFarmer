using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] dontDestroyOnLoadObjects;

    [Space]
    [SerializeField] private GameObject farmingPlots;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        foreach (GameObject go in dontDestroyOnLoadObjects)
        {
            DontDestroyOnLoad(go);
        }
    }

    public void OnQuitLevel(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (SceneManager.GetActiveScene().buildIndex == 0) { ToDungeon(); }
            else if (SceneManager.GetActiveScene().buildIndex == 1) { ToFarm(); }
        }
    }

    private void ToDungeon()
    {
        SceneManager.LoadScene(1);
        GetComponent<InventoryManager>().SavePlayerInventory();
        farmingPlots.SetActive(false);
    }


    private void ToFarm()
    {
        SceneManager.LoadScene(0);
        GetComponent<InventoryManager>().LoadPlayerInventory();
        farmingPlots.SetActive(true);
    }
}
