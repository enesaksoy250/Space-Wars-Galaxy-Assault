using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource playerFireAudioSource;
    public AudioSource enemyFireAudioSource;

    [Space()]
    [Header("Audio Clip")]
    [SerializeField] AudioClip gameStartClip;                              
    [SerializeField] AudioClip gameOverClip;
    [SerializeField] AudioClip errorClip;
    [SerializeField] AudioClip menuSelectClip;
    [SerializeField] AudioClip purchaseClip;
    [SerializeField] AudioClip pickupClip;
    [SerializeField] AudioClip explosionClip;

    [HideInInspector]public AudioSource audioSource;

    public static AudioManager instance;

    private void Awake()
    {
       
        if (instance == null)
        {

            instance = this;
            DontDestroyOnLoad(gameObject);
        
        }
            

        else
        {

            Destroy(gameObject);

        }

        audioSource = GetComponent<AudioSource>();

    }

   
    public void PlayAudio(string audioName)
    {

        switch(audioName)
        {

            case "gameStart":
                audioSource.PlayOneShot(gameStartClip);
                break;

            case "playerFire":
                playerFireAudioSource.Play();
                break;

            case "enemyFire":
                enemyFireAudioSource.Play();
                break;

            case "gameOver":
                audioSource.PlayOneShot(gameOverClip);
                break;

            case "error":
                audioSource.PlayOneShot(errorClip);
                break;

            case "select":
                audioSource.PlayOneShot(menuSelectClip);
                break;

            case "purchase":
                audioSource.PlayOneShot(purchaseClip);
                break;

            case "pickup":
                audioSource.PlayOneShot(pickupClip);
                break;

            case "explosion":
                audioSource.PlayOneShot(explosionClip);
                break;

        }


    }

}
