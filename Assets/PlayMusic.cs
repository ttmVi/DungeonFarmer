using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    public string musicName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(musicName == "FarmMusic")
            {
                AudioManager.instance.SwitchBGM(AudioManager.instance.FarmMusic);
            }
            else if(musicName == "DungeonMusic")
            {
                AudioManager.instance.SwitchBGM(AudioManager.instance.DungeonMusic);
            }
            else if(musicName == "LavaMusic")
            {
                AudioManager.instance.SwitchBGM(AudioManager.instance.LavaMusic);
            }
            else if(musicName == "RestMusic")
            {
                AudioManager.instance.SwitchBGM(AudioManager.instance.RestMusic);
            }
            else if(musicName == "BossMusic")
            {
                AudioManager.instance.SwitchBGM(AudioManager.instance.BossMusic);
            }
            else if(musicName == "VictoryMusic")
            {
                AudioManager.instance.SwitchBGM(AudioManager.instance.VictoryMusic);
            }
            
        }
    }
}
