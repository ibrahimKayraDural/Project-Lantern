using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LevelEnder : MonoBehaviour
{
    [SerializeField] Collider2D col;

    void Start()
    {
        if (col == null) col = TryGetComponent(out Collider2D outCollider) ? outCollider : null;
        if (col != null) col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && LevelManager.instance != null)
        {
            if(LevelManager.instance.GoToNextLevel() == false)
            {
                LevelManager.instance.GoToLevel(0);
            }
        }
    }
}
