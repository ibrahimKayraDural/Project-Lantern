using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToMousePosition : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] float lerpSpeed = 5f;

    GameObject tracker;

    void Start()
    {
        tracker = new GameObject("tracker");
        tracker.transform.parent = transform.parent;
        tracker.transform.localPosition = transform.localPosition;
    }
    void Update()
    {
        tracker.transform.localPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.localPosition = 
            Vector2.Lerp(transform.localPosition, tracker.transform.localPosition, Mathf.Clamp(Time.deltaTime * lerpSpeed, 0, 1));
    }
}
