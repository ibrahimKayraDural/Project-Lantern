using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class SpawnTrigger : MonoBehaviour
{
    [SerializeField] Spawner spawnerRef;

    void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        rb.gravityScale = 0;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (spawnerRef != null)
            {
                spawnerRef.SpawnEnemy();
            }
        }
    }
}
