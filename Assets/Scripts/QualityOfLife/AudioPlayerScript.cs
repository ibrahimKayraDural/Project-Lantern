using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioPlayerScript : MonoBehaviour
{
    [System.Serializable] struct LevelSong
    {
        public int[] levelIndexesToPlay;
        public AudioClip songToPlay;
        public bool dontLoop;
        public bool dontPersist;
    }

    public static AudioPlayerScript instance;
    [SerializeField] LevelSong[] levelSongs;
    [SerializeField] LevelSong currentLevelSong;
    [SerializeField] AudioSource audioSource;

    int oldIndex = -1;

    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            CheckSong(SceneManager.GetActiveScene().buildIndex);
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }
    }

    void SceneManager_sceneLoaded(Scene level, LoadSceneMode arg1)
    {
        CheckSong(level.buildIndex);
    }

    AudioClip CheckSong(int level)
    {
        foreach (LevelSong LS in levelSongs)
        {
            foreach (int indx in LS.levelIndexesToPlay)
            {
                if (indx == level)
                {
                    if (level != oldIndex)
                    {
                        audioSource.Stop();
                        audioSource.clip = LS.songToPlay;
                        audioSource.loop = !LS.dontLoop;
                        currentLevelSong = LS;
                        audioSource.Play();
                        oldIndex = level;
                        return audioSource.clip;
                    }
                }
            }
        }

        if (currentLevelSong.dontPersist)
        {
            audioSource.Stop();
        }

        return null;
    }
}
