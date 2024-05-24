using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private TutorialManager manager;

    [TextArea(5,10)]
    [SerializeField] private string tutorialContent;

    private void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<TutorialManager>();
    }

    public void TriggerTutorial()
    {
        manager.OpenTutorial(tutorialContent);
    }
}
