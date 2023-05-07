using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CallEvent : MonoBehaviour
{
    [SerializeField] UnityEvent events;
    public void CallIt()
    {
        events.Invoke();
    }
}
