using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyScript : MonoBehaviour
{
    [SerializeField] UnityEvent pickupEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            pickupEvent?.Invoke();
            Destroy(gameObject);
        }
    }
}
