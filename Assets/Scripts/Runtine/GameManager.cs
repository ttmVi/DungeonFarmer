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
    [SerializeField] private GameObject player;

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
        player.transform.position = new Vector3(-56f, 17f, 0f);
    }


    private void ToFarm()
    {
        SceneManager.LoadScene(0);
        GetComponent<InventoryManager>().LoadPlayerInventory();
        farmingPlots.SetActive(true);
        player.transform.position = new Vector3(11.5f, -5.5f, 0f);
    }
}
