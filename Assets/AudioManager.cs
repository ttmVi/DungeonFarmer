using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource BGM;
    private GameManager gameManager;
    public AudioClip FarmMusic;
    public AudioClip DungeonMusic;
    public AudioClip LavaMusic;
    public AudioClip RestMusic;
    public AudioClip BossMusic;
    public AudioClip VictoryMusic;
    public AudioSource JumpSound;
    public AudioSource AttackSound;
    public AudioSource DashSound;
    public AudioSource HurtSound;
    public AudioClip Jump;
    public AudioClip Attack;
    public AudioClip Dash;
    public AudioClip Hurt;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
       gameManager=GetComponent<GameManager>();
       BGM = GetComponent<AudioSource>();
        JumpSound.clip=Jump;
        AttackSound.clip=Attack;
        DashSound.clip=Dash;
        HurtSound.clip=Hurt;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchBGM(AudioClip music)
    {
        StartCoroutine(FadeOut(BGM,2f,music));
        
    }
    public void PlayBGM(AudioClip music)
    {
        BGM.Stop();
        BGM.clip = music;
        BGM.Play();
    }
    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime,AudioClip music)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
        audioSource.clip = music;
        audioSource.Play();
    }
}
