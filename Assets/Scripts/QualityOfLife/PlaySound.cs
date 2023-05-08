using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySound : MonoBehaviour
{
    [SerializeField] AudioSource aSource;
    [SerializeField] AudioClip optionalClip;
    [SerializeField] float cooldown = 0;
    [SerializeField] bool playLooping = false;
    [SerializeField] float pitchRandomizerMin = 0;
    [SerializeField] float pitchRandomizerMax = 0;

    float nextOverridePlayTargetTime = float.MinValue;

    void Start()
    { 
        if(aSource == null) aSource = GetComponent<AudioSource>();
        if (aSource == null) Debug.LogError("No audio source has been given to me :(");
    }

    public void PlayWithDefaults()
    {
        if (aSource == null) return;
        if (optionalClip == null) return;
        if (nextOverridePlayTargetTime > Time.time) return;

        aSource.Stop();

        aSource.clip = optionalClip;
        aSource.loop = playLooping;

        if (pitchRandomizerMax < pitchRandomizerMin)
        {
            pitchRandomizerMax = 0;
            pitchRandomizerMin = 0;
        }
        float randomPitch = Random.Range(pitchRandomizerMin, pitchRandomizerMax);
        aSource.pitch = 1 + randomPitch;

        aSource.Play();

        nextOverridePlayTargetTime = Time.time + cooldown;
    }
    public void Play(AudioClip clip, float cooldown, bool playLooping = false, float pitchRandomizerMin = 0, float pitchRandomizerMax = 0)
    {
        if (nextOverridePlayTargetTime > Time.time) return;

        aSource.Stop();

        aSource.clip = clip;
        aSource.loop = playLooping;

        if (pitchRandomizerMax < pitchRandomizerMin)
        {
            pitchRandomizerMax = 0;
            pitchRandomizerMin = 0;
        }
        float randomPitch = Random.Range(pitchRandomizerMin, pitchRandomizerMax);
        aSource.pitch = 1 + randomPitch;

        aSource.Play();

        nextOverridePlayTargetTime = Time.time + cooldown;
    }
    public void Play(float cooldown, bool playLooping = false, float pitchRandomizerMin = 0, float pitchRandomizerMax = 0)
    {
        if (nextOverridePlayTargetTime > Time.time) return;

        if(optionalClip == null)
        {
            if(aSource.clip == null)
            {
                return;
            }
            optionalClip = aSource.clip;
        }

        aSource.Stop();

        aSource.clip = optionalClip;
        aSource.loop = playLooping;

        if(pitchRandomizerMax < pitchRandomizerMin)
        {
            pitchRandomizerMax = 0;
            pitchRandomizerMin = 0;
        }
        float randomPitch = Random.Range(pitchRandomizerMin, pitchRandomizerMax);
        aSource.pitch = 1 + randomPitch;

        aSource.Play();

        nextOverridePlayTargetTime = Time.time + cooldown;
    }
}
