using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [Space]
    [Header("Scene Changing Manager")]
    [SerializeField] private GameObject SceneChangingCanvas;
    [SerializeField] private Image blackImage;
    [SerializeField] private Color black;
    [SerializeField] private Color clear;

    [Space]
    [Header("Scene Changing Variables")]
    [SerializeField] private Vector2 firstEverStartingPos;
    [Space]
    [SerializeField] public GameObject farm;
    [SerializeField] public GameObject dungeon;
    [SerializeField] public bool inFarm = true;
    [SerializeField] public bool inDungeon = false;
    [SerializeField] private Vector2 farmStartPos;
    [SerializeField] private Vector2 dungeonStartPos;
    [Space]
    [SerializeField] private GameObject farmDoor;
    [SerializeField] private GameObject dungeonDoor;
    [SerializeField] private AnimationClip doorOpen;
    [SerializeField] private AnimationClip doorClose;
    [SerializeField] private Sprite openedDoor;
    [SerializeField] private Sprite closedDoor;

    [Space]
    [Header("Dungeon Variables")]
    [SerializeField] private Transform startMagmaTrigger;
    [SerializeField] private Transform stopMagmaTrigger;
    [SerializeField] private GameObject magma;
    [SerializeField] private GameObject enemies;
    private GameObject currentEnemies;

    private void Start()
    {
        player.transform.position = firstEverStartingPos;
        farm.SetActive(true);
        inFarm = true;
        player.GetComponent<PlayerAttack>().enabled = false;
        player.transform.GetChild(0).GetComponent<Melee>().enabled = false;

        currentEnemies = GameObject.Find("Enemies");
        dungeon.SetActive(false);
        inDungeon = false;
    }

    private void Update()
    {
        if (inDungeon && player.transform.position.y >= startMagmaTrigger.position.y && Mathf.Abs(player.transform.position.x - startMagmaTrigger.position.x) <= 7)
        {
            if (magma.transform.position.y >= stopMagmaTrigger.position.y)
            {
                magma.GetComponent<RisingMagma>().enabled = false;
            }
            else { magma.GetComponent<RisingMagma>().enabled = true; }
        }
    }

    private void ResetDungeon()
    {
        magma.GetComponent<RisingMagma>().ResetPosition();
        magma.GetComponent<RisingMagma>().enabled = false;
        if (currentEnemies != null)
        {
            Destroy(currentEnemies);
            currentEnemies = Instantiate(enemies);
            currentEnemies.transform.parent = dungeon.transform;
            Debug.Log(currentEnemies.gameObject.name);
        }

        if (currentEnemies == null)
        {
            currentEnemies = Instantiate(enemies);
            currentEnemies.transform.parent = dungeon.transform;
            Debug.Log(currentEnemies.gameObject.name);
        }
    }

    private void DestroyEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemies");
        foreach (GameObject enemy in enemies) { Destroy(enemy); }
    }

    private IEnumerator ShiftToDungeon(GameObject openingDoor)
    {
        DisablePlayer();
        if (openingDoor != null)
        {
            openingDoor.GetComponent<Animator>().SetTrigger("doorOpen");
            yield return new WaitForSeconds(doorOpen.length);
        }
        else { yield return new WaitForEndOfFrame(); }

        if (openingDoor != null)
        {
            openingDoor.GetComponent<SpriteRenderer>().sprite = openedDoor;
        }
        StartCoroutine(BlackOut());
        yield return new WaitUntil(() => ScreenIsBlack());

        farm.SetActive(false);
        inFarm = false;

        player.transform.position = dungeonStartPos;
        dungeon.SetActive(true);
        inDungeon = true;
        dungeonDoor.GetComponent<SpriteRenderer>().sprite = openedDoor;
        ResetDungeon();
        yield return new WaitForSeconds(2f);

        StartCoroutine(WhiteIn());
        yield return new WaitUntil(() => ScreenIsClear());

        dungeonDoor.GetComponent<Animator>().SetTrigger("doorClose");
        yield return new WaitForSeconds(doorClose.length);

        dungeonDoor.GetComponent<SpriteRenderer>().sprite = closedDoor;
        EnablePlayer();
    }

    public void ToDungeon(GameObject openingDoor) 
    {
        StartCoroutine(ShiftToDungeon(openingDoor)); 
    }

    private IEnumerator ShiftToFarm(GameObject openingDoor)
    {
        DisablePlayer();

        if (openingDoor != null)
        {
            openingDoor.GetComponent<Animator>().SetTrigger("doorOpen");
            yield return new WaitForSeconds(doorOpen.length);
        }

        if (openingDoor != null)
        {
            openingDoor.GetComponent<SpriteRenderer>().sprite = openedDoor;
        }
        player.GetComponent<PlayerInventory>().StoreItemsFromDungeon();

        StartCoroutine(BlackOut());
        yield return new WaitUntil(() => ScreenIsBlack());

        DestroyEnemies();
        dungeon.SetActive(false);
        inDungeon = false;

        player.transform.position = farmStartPos;
        farm.SetActive(true);
        inFarm = true;
        farmDoor.GetComponent<SpriteRenderer>().sprite = openedDoor;
        player.GetComponent<PlayerAnimationController>().ResetPlayerAnimation();
        player.GetComponent<BoxCollider2D>().enabled = true;
        //player.GetComponent<PlayerInventory>().StoreItemsFromDungeon();
        if (player.GetComponent<PlayerHealth>().IsDying())
        {
            player.GetComponent<PlayerHealth>().StopDying();
        }

        GetComponent<FarmManager>().ForcedEndFarmDay();
        yield return new WaitForSeconds(2f);

        StartCoroutine(WhiteIn());
        yield return new WaitUntil(() => ScreenIsClear());

        farmDoor.GetComponent<Animator>().SetTrigger("doorClose");
        yield return new WaitForSeconds(doorClose.length);

        farmDoor.GetComponent<SpriteRenderer>().sprite = closedDoor;
        EnablePlayer();
        player.GetComponent<PlayerAttack>().enabled = false;
        player.transform.GetChild(0).GetComponent<Melee>().enabled = false;
        //player.GetComponent<PlayerDash>().enabled = false;
    }

    public void ToFarm(GameObject openingDoor) 
    {
        StartCoroutine(ShiftToFarm(openingDoor)); 
    }

    public IEnumerator WhiteIn()
    {
        float time = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            blackImage.color = Color.Lerp(black, clear, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        blackImage.color = clear;
        EnablePlayer();
        if (inFarm)
        {
            player.GetComponent<PlayerAttack>().enabled = false;
            player.transform.GetChild(0).GetComponent<Melee>().enabled = false;
        }
        yield return null;
    }

    public IEnumerator BlackOut()
    {
        float time = 0.5f;
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
        player.GetComponent<PlayerDash>().enabled = false;
        player.GetComponent<PlayerAttack>().enabled = false;
        player.transform.GetChild(0).GetComponent<Melee>().enabled = false;
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    private void EnablePlayer()
    {
        player.GetComponent<PlayerInteract>().enabled = true;
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<PlayerJump>().enabled = true;
        player.GetComponent<PlayerDash>().enabled = true;
        player.GetComponent<PlayerAttack>().enabled = true;
        player.transform.GetChild(0).GetComponent<Melee>().enabled = true;
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }
}
