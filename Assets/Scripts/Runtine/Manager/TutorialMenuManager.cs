using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialMenuManager : MonoBehaviour
{
    [SerializeField] private Image blackImage;
    [SerializeField] private Color black;
    [SerializeField] private Color clear;

    [Space]
    [SerializeField] private TextMeshProUGUI tutorialText;
    [TextArea(5, 10)]
    [SerializeField] private string[] tutorials;
    [SerializeField] private int currentTutorial;

    private Coroutine loadGameCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        loadGameCoroutine = null;
        tutorialText.text = tutorials[0];
        currentTutorial = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && loadGameCoroutine == null)
        {
            loadGameCoroutine = StartCoroutine(LoadGame());
        }
    }

    private IEnumerator LoadGame()
    {
        blackImage.gameObject.SetActive(true);
        float time = 0.5f;
        float elapsedTime = 0f;
        //DisablePlayer();

        while (elapsedTime < time)
        {
            blackImage.color = Color.Lerp(clear, black, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        blackImage.color = black;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        yield return null;
    }

    public void NextTutorial()
    {
        if (loadGameCoroutine == null && currentTutorial != (tutorials.Length - 1))
        {
            currentTutorial++;
            tutorialText.text = tutorials[currentTutorial];
        }
    }

    public void PreviousTutorial()
    {
        if (loadGameCoroutine == null && currentTutorial != 0)
        {
            currentTutorial--;
            tutorialText.text = tutorials[currentTutorial];
        }
    }
}
