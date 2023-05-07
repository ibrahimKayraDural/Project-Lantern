using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWithEvent : MonoBehaviour
{
    [SerializeField] GameObject[] OptionalGameObjectsToDestroy;

    public void DestroyViaEvent()
    {
        foreach(GameObject go in OptionalGameObjectsToDestroy)
        {
            Destroy(go);
        }

        Destroy(gameObject);
    }
}
