using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundtrack : MonoBehaviour
{
    private AudioSource _audioSource;
    private bool music_on;
    public static Soundtrack Instance = null;

    private void Awake()
    {
        _audioSource = _audioSource = GetComponent<AudioSource>();

        if (Instance == null)
        {
            Instance = this;
        }

        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);

        if (Instance.music_on){
            if (!Instance._audioSource.isPlaying)
            {
                Instance._audioSource.Play();
            }
        }
    }

    public void toggle_music(){
        Debug.Log("blabla");
        Instance.music_on = !Instance.music_on;
        if (Instance.music_on){
            Instance.PlayMusic();
        } else{
            Instance.StopMusic();
        }
    }

    public void PlayMusic()
    {
        if (_audioSource.isPlaying) return;
        _audioSource.Play();
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }
}