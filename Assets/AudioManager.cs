using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource BGM;
    private GameManager gameManager;
    public AudioClip FarmMusic;
    public AudioClip DungeonMusic;
    public AudioClip LavaMusic;
    public AudioClip RestMusic;
    public AudioClip BossMusic;
    public AudioClip VictoryMusic;
    // Start is called before the first frame update
    void Start()
    {
       gameManager=GetComponent<GameManager>();
       BGM = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.inFarm && BGM.clip != FarmMusic)
        {
            BGM.clip = FarmMusic;
            BGM.Play();
        }
    }

    public void SwitchBGM(AudioClip music)
    {
        StartCoroutine(FadeOut(BGM,2f));
        BGM.clip = music;
        BGM.Play();
    }
    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
