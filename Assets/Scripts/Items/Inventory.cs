using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public float KeyCount;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetKey()
    {
        KeyCount++;
    }
     
    public void UseKey()
    {
        KeyCount--;
    }
}
