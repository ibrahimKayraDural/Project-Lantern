using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbushAudio : MonoBehaviour
{
    Transform PlayerTransform;
    [SerializeField] float AudioRange = 10;
    [SerializeField] float MaxAudio = 1;
    [SerializeField] AudioSource AS;
    void Start()
    {
        PlayerTransform = GameManager.instance.GetPlayerTransform();
    }

    // Update is called once per frame
    void Update()
    {
        float TargetDistance = Vector2.Distance(PlayerTransform.position, transform.position);
        TargetDistance = Mathf.Clamp(TargetDistance, 0, AudioRange);
        TargetDistance = Mathf.InverseLerp(0, AudioRange, TargetDistance);
        TargetDistance = 1 - TargetDistance;

        AS.volume = Mathf.Lerp(0, MaxAudio, TargetDistance);

        if(AS.isPlaying == false)
        {
            ReplayAudio();
        }
    }

    void ReplayAudio()
    {
        AS.Play();
    }
}
