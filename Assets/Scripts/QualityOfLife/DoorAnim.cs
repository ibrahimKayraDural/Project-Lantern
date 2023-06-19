using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Animations;

public class DoorAnim : MonoBehaviour
{
    [SerializeField] Animator Anim;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    OpenDoor();

        //}
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<Inventory>().KeyCount>0)
            {
                collision.gameObject.GetComponent<Inventory>().UseKey();
                OpenDoor();
            }

        }
    }

    void OpenDoor()
    {
        Anim.SetBool("DoorOpen", true);
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
