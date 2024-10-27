using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class AudioVolumeSetter : MonoBehaviour
{
   
     AudioSource[] audioSources;

    private void Awake()
    {
    
        audioSources = FindObjectsOfType<AudioSource>();

    }

    private void Start()
    {
        SetAudioVolume();
    }

    private void SetAudioVolume()
    {

        foreach (AudioSource source in audioSources)
        {

            if (source.gameObject.name == "AudioManager")
            {
                PlayerPrefsControl("Sound");
                PlayerPrefsControl("FireSound");                      
                AudioManager.instance.audioSource.volume = PlayerPrefs.GetFloat("Sound")/2;
                AudioManager.instance.playerFireAudioSource.volume = PlayerPrefs.GetFloat("FireSound")/4;
                AudioManager.instance.enemyFireAudioSource.volume  = PlayerPrefs.GetFloat("FireSound")/16;
            
            }

            else if (source.gameObject.name == "MainMenuMusic")
            {

                PlayerPrefsControl("MainMenuMusic");
                source.volume = PlayerPrefs.GetFloat("MainMenuMusic")/2;

            }

            else if (source.gameObject.name == "GameMusic")
            {

                PlayerPrefsControl("GameMusic");
                source.volume = PlayerPrefs.GetFloat("GameMusic")/2;

            }

        }

    }

    private void PlayerPrefsControl(string name)
    {

        if(!PlayerPrefs.HasKey(name))
            PlayerPrefs.SetFloat(name, .5f);
                   
    }

}
