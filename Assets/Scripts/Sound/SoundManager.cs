using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private AudioSource musicSource;
    private AudioSource effectsSource;
    private string currentMusic;

    [Space]
    [SerializeField] Sound[] music = default;
    [SerializeField] Sound[] effects = default;



    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        musicSource = GetComponents<AudioSource>()[0];
        effectsSource = GetComponents<AudioSource>()[1];

    }

    public bool IsPlayingMusic(string musicName)
    {
        return musicSource.isPlaying && musicName == currentMusic;
    }


    public void PlayMusic(string name)
    {
        Sound s = Array.Find(music, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Music: " + name + " not found!");
            return;
        }
        musicSource.clip = s.clip;
        musicSource.Play();
        currentMusic = name;
    }

    public void StopMusic()
    {
        musicSource.Stop();
        musicSource.clip = null;
        currentMusic = "";
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void ResumeMusic()
    {
        musicSource.Play();
    }

    public void PlayEffect(string name)
    {
        Sound s = Array.Find(effects, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Effect: " + name + " not found!");
            return;
        }
        effectsSource.clip = s.clip;
        effectsSource.PlayOneShot(effectsSource.clip);
    }
    public void StopEffect()
    {
        effectsSource.Stop();
        effectsSource.clip = null;
        
    }
    public float GetMusicVolume()
    {
        return musicSource.volume;
    }
    public void SetMusicVolume(float _volume)
    {
        musicSource.volume = _volume;
    }

    public float GetEffectsVolume()
    {
        return effectsSource.volume;
    }
    public void SetEffectsVolume(float _volume)
    {
        effectsSource.volume = _volume;
    }

    public void SetSheepsVolume(float _volume)
    {
        GameObject[] sheeps = GameObject.FindGameObjectsWithTag("Sheep");
        foreach(GameObject sheep in sheeps)
        {
            AudioSource sheepAudio = sheep.GetComponent<AudioSource>();
            if(sheepAudio != null)
            {
                sheepAudio.volume = _volume;
            }
        }
    }

    public void SetSheepsPackVolume(float _volume)
    {
        GameObject.Find("Sheeps").GetComponent<AudioSource>().volume = _volume;
    }

}
