using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    [SerializeField] GameObject Door;
    [SerializeField] GameObject sideEntry;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Door !=null)
            {
               Destroy(Door);
            }
            else if (sideEntry != null)
            {
                sideEntry.GetComponent<BoxCollider2D>().enabled = false;
            }

            Destroy(gameObject);
        }
    }
}
