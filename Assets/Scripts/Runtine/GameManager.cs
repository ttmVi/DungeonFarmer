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

    [SerializeField] private Vector2 farmStartPos;
    [SerializeField] private Vector2 dungeonStartPos;
        
    public void OnQuitLevel(InputAction.CallbackContext context)
    {

    }

    private IEnumerator ShiftToDungeon()
    {
        DisablePlayer();
        StartCoroutine(BlackOut());
        yield return new WaitUntil(() => ScreenIsBlack());

        player.transform.position = dungeonStartPos;
        yield return new WaitForSeconds(0.5f);

        StartCoroutine(WhiteIn());
        yield return new WaitUntil(() => ScreenIsClear());
        EnablePlayer();
    }

    private IEnumerator ShiftToFarm()
    {
        DisablePlayer();
        StartCoroutine(BlackOut());
        yield return new WaitUntil(() => ScreenIsBlack());

        player.transform.position = farmStartPos;
        yield return new WaitForSeconds(0.5f);

        StartCoroutine(WhiteIn());
        yield return new WaitUntil(() => ScreenIsClear());
        EnablePlayer();
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
