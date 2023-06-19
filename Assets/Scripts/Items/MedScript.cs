using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] AudioSource AS;
    bool Picked;
    void Start()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<TopDownCharacterController>().HealPlayer();
            AS.Play();

            Picked = true;
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Picked && !AS.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
