using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AudioVolumeSlider : MonoBehaviour,IPointerUpHandler
{

    Slider slider;
    private string sliderName;

    private void Awake()
    {

        slider = GetComponent<Slider>();
        sliderName = gameObject.name;
        SetSliderValue();
       
    }

  
    private void SetSliderValue()
    {

        string name = gameObject.name.Substring(0,gameObject.name.Length - 6);
        slider.value = PlayerPrefs.GetFloat(name);   

    }

    public void OnPointerUp(PointerEventData eventData)
    {
     
        switch (gameObject.name)
        {

            case "SoundSlider":
                AudioManager.instance.audioSource.volume = slider.value/2;
                PlayerPrefs.SetFloat("Sound", slider.value);
                break;

            case "MainMenuMusicSlider":
                GameObject.FindWithTag("MainMenuMusic").GetComponent<AudioSource>().volume = slider.value/2;
                PlayerPrefs.SetFloat("MainMenuMusic", slider.value);
                break;

            case "GameMusicSlider":
                GameObject.FindWithTag("GameMusic").GetComponent<AudioSource>().volume = slider.value/2;
                PlayerPrefs.SetFloat("GameMusic", slider.value);
                break;

            case "FireSoundSlider":
                AudioManager.instance.playerFireAudioSource.volume = slider.value/4;
                AudioManager.instance.enemyFireAudioSource.volume = slider.value/16;
                PlayerPrefs.SetFloat("FireSound", slider.value);
                break;

        }
    }
}
