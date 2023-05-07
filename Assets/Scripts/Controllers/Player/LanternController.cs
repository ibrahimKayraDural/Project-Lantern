using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] Transform origin;

    [Header("Values")]
    [SerializeField] float range = 2f;
    [SerializeField] float trackerSpeed = 10f;
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
        Vector3 mouseDeltaInput = new Vector3(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"), 0) * trackerSpeed / 100;

        Vector3 targetTransform = tracker.transform.localPosition + mouseDeltaInput;
        tracker.transform.localPosition = Vector2.ClampMagnitude(targetTransform, range + 1);

        //reduce, reuse, ecyce
        targetTransform = Vector2.Lerp(transform.localPosition, tracker.transform.localPosition, Mathf.Clamp(Time.deltaTime * lerpSpeed, 0, 1));
        transform.localPosition = Vector2.ClampMagnitude(targetTransform, range);
    }
}
