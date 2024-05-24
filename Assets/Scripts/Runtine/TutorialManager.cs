using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [Header("Tutorial Canvas Elements Assigning")]
    [SerializeField] private GameObject tutorialCanvas;
    [SerializeField] private TextMeshProUGUI tutorialTitle;
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] private bool isTutoring;

    private void Update()
    {
        if (isTutoring)
        {
            player.GetComponent<PlayerInteract>().enabled = false;
            player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponent<PlayerJump>().enabled = false;
            player.GetComponent<PlayerAttack>().enabled = false;
            player.GetComponent<PlayerDash>().enabled = false;
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    public void OpenTutorial(string title, string tutorialContent)
    {
        tutorialCanvas.SetActive(true);
        tutorialTitle.text = title;
        tutorialText.text = tutorialContent;
        isTutoring = true;

        player.GetComponent<PlayerInteract>().enabled = false;
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<PlayerJump>().enabled = false;
        player.GetComponent<PlayerAttack>().enabled = false;
        player.GetComponent<PlayerDash>().enabled = false;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void CloseTutorial()
    {
        tutorialCanvas.SetActive(false);
        tutorialTitle.text = " ";
        tutorialText.text = " ";
        isTutoring = false;

        if (!GetComponent<InventoryManager>().CheckGeneralItemsUIOpeningState())
        {
            player.GetComponent<PlayerAnimationController>().ResetTriggerInteractingAnimation();
            player.GetComponent<PlayerInteract>().enabled = true;
            player.GetComponent<PlayerMovement>().enabled = true;
            player.GetComponent<PlayerDash>().enabled = true;
            player.GetComponent<PlayerJump>().enabled = true;
        }
    }

    public void CloseTutorial(InputAction.CallbackContext context)
    {
        if (context.started && isTutoring)
        {
            CloseTutorial();
        }
    }
}
