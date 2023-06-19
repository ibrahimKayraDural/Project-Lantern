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
    void OpenDoor()
    {
        Anim.SetBool("DoorOpen", true);
    }
}
