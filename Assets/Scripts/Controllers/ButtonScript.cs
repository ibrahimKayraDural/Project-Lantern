using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Material normalMat;
    [SerializeField] Material highlightedMat;

    [SerializeField] UnityEvent OnClicked;

    bool isHighlighted;

    void Update()
    {
        if(isHighlighted && Input.GetMouseButtonDown(0))
        {
            OnClicked?.Invoke();
        }
    }
    void OnMouseEnter()
    {
        sr.material = highlightedMat;
        isHighlighted = true;
    }
    void OnMouseExit()
    {
        sr.material = normalMat;
        isHighlighted = false;
    }
}
