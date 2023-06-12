using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionGridTest : MonoBehaviour
{
    [SerializeField] GameObject collisionGridPrefab;
    [SerializeField] Vector2 size = Vector2.one * 10;

    void Start()
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {

                Vector2 targetPos = transform.position;
                targetPos.x += collisionGridPrefab.transform.lossyScale.x * x;
                targetPos.y += collisionGridPrefab.transform.lossyScale.y * y;

                Instantiate(collisionGridPrefab, targetPos, Quaternion.identity, transform);

            }
        }
    }
}
