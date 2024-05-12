using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    //REF: https://discussions.unity.com/t/how-to-check-which-scene-is-loaded-and-write-if-code-for-it/163399/2
    //REF: https://www.youtube.com/watch?v=DU7cgVsU2rM

    //VOLUME VARIABLES
    [SerializeField] private AudioMixer audioMixer;
    //private float masterVolume = 100F;
    //private float sfxVolume = 100F;
    //private float musicVolume = 100F;

    //MUSIC AUDIO SOURCE
    private AudioSource currentMusicSource; //MUSIC IS CHILD INDEX 0
    [SerializeField] AudioSource sfxSource;

    //MUSIC CLIP VARIABLES
    [SerializeField] AudioClip[] musicClips;    // 0 - MainMenu, 1 - Gameplay

    //SFX CLIP VARIABLES
    [SerializeField] AudioClip[] sfxClips;        // 0 - IntroDoor

    //AUDIO BOOL CHECKS
    private bool musicIsPlaying = false;

    void Start()
    {
        currentMusicSource = transform.GetChild(0).GetComponent<AudioSource>();
        SetMasterVolume(0.0001F);
    }

    void Update()
    {
        //MAIN MENU - MUSIC
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (currentMusicSource.clip != musicClips[0]) {
                stopMusic();
            }
            if (!musicIsPlaying)
            {
                playMusic(musicClips[0], 10F);
            }        
        }

        //CHANGES MUSIC WHEN GAME LAUNCHES
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (currentMusicSource.clip != musicClips[1])
            {
                stopMusic();
            }
                if (!musicIsPlaying)
                {
                    playMusic(musicClips[1], 10F);
                }
        }
        if (currentMusicSource.time >= currentMusicSource.clip.length)
        {
            stopMusic();
        }
    }

    //===MIXER CONTROLLERS===
    public void SetMasterVolume(float level) {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level)*20F);
    }
    public void SetSFXVolume(float level)
    {
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(level) * 20F);
    }
    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20F);
    }

    //===MUSIC CONTROLLERS===
    public void playMusic(AudioClip audio, float volume) {

        currentMusicSource.clip = audio;
        currentMusicSource.volume = volume;
        currentMusicSource.Play();
        musicIsPlaying = true;
    }
    public void stopMusic()
    {
        currentMusicSource.Stop();
        musicIsPlaying = false;
    }

    //===SFX CONTROLLERS===
    //CREATES + DESTROYS AUDIO OBJECTS
    public void spawnSFX(AudioClip audio, Transform spawn, float volume) {
        AudioSource audioSource = Instantiate(sfxSource, spawn.position, Quaternion.identity);
        audioSource.clip = audio;
        audioSource.volume = volume;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    public void playDoorSFX() {
        spawnSFX(sfxClips[0], transform, 1F);
    }
}
