using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] dontDestroyOnLoadObjects;

    [Space]
    [SerializeField] private GameObject farmingPlots;
    [SerializeField] private GameObject player;

    [Space]
    [Header("Scene Changing Manager")]
    [SerializeField] private GameObject SceneChangingCanvas;
    [SerializeField] private Image blackImage;
    [SerializeField] private Color black;
    [SerializeField] private Color clear;
        
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

    public IEnumerator WhiteIn()
    {
        float time = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            blackImage.color = Color.Lerp(black, clear, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        blackImage.color = clear;
        EnablePlayer();
        yield return null;
    }

    public IEnumerator BlackOut()
    {
        float time = 1f;
        float elapsedTime = 0f;
        DisablePlayer();

        while (elapsedTime < time)
        {
            blackImage.color = Color.Lerp(clear, black, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        blackImage.color = black;
        yield return null;
    }

    public bool ScreenIsBlack() { return blackImage.color == black; }

    public bool ScreenIsClear() { return blackImage.color == clear; }

    private void DisablePlayer()
    {
        player.GetComponent<PlayerInteract>().enabled = false;
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<PlayerJump>().enabled = false;
        player.GetComponent<PlayerAttack>().enabled = false;
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    private void EnablePlayer()
    {
        player.GetComponent<PlayerInteract>().enabled = true;
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<PlayerJump>().enabled = true;
        player.GetComponent<PlayerAttack>().enabled = true;
    }
}
