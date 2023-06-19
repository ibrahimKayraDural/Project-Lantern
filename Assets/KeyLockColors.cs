using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyLockColors : MonoBehaviour
{
    [SerializeField] Color KeyLockColor = Color.white;

    [Header("Reference")]
    [SerializeField] SpriteRenderer KeyRenderer;
    [SerializeField] SpriteRenderer LockRenderer;

    void OnValidate()
    {
        KeyRenderer.color = KeyLockColor;
        LockRenderer.color = KeyLockColor;
    }
}
