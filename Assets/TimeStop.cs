using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStop : MonoBehaviour
{
    private float speed;
    private bool RestoreTime;
    // Start is called before the first frame update
    void Start()
    {
        RestoreTime = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(RestoreTime)
        {
            if(Time.timeScale < 1)
            {
                Time.timeScale += Time.deltaTime * speed;
            }
            else
            {
                Time.timeScale = 1;
                RestoreTime = false;
            }
        }
    }
    public void StopTime(float changeTime, int RestoreSpeed, float delay)
    {
        speed = RestoreSpeed;
        if (delay > 0)
        {
            StopCoroutine(StartTimeAgain(delay));
            StartCoroutine(StartTimeAgain(delay));
        }
        else
        {
            RestoreTime = true;
        }
        Time.timeScale = changeTime;
    }
    IEnumerator StartTimeAgain(float delay)
    {
        
        RestoreTime = true;
        yield return new WaitForSecondsRealtime(delay);
    }
}
