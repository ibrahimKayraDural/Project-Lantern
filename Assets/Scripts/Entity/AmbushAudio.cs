using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbushAudio : MonoBehaviour
{
    Vector2 PPos;
    float cumDistance;
    [SerializeField] float AudioRange;
    [SerializeField] AudioSource AS;
    void Start()
    {
        PPos = GameManager.instance.GetPlayerPosition();
    }

    // Update is called once per frame
    void Update()
    {
        cumDistance = gameObject.transform.position.magnitude - PPos.magnitude;
        if (cumDistance < AudioRange)
        {
            AS.Play();
        }
    }
}
