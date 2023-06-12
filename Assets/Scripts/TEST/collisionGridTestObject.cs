using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionGridTestObject : MonoBehaviour
{
    public void ChangeColorForSeconds(Color color, float seconds = 1)
    {
        if(TryGetComponent(out SpriteRenderer sr))
        {
            sr.color = color;
            Invoke("TurnWhite", seconds);
        }
    }

    void TurnWhite()
    {
        if (TryGetComponent(out SpriteRenderer sr))
        {
            sr.color = Color.white;
        }
    }
}