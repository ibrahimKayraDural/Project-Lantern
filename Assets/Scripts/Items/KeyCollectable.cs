using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCollectable : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] AudioSource AS;
    bool picked;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (picked && !AS.isPlaying)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Inventory>().GetKey();
            AS.Play();

            picked = true;
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
