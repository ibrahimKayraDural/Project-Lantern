using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWallCollision : MonoBehaviour
{
    [SerializeField] GameObject collision;
    [SerializeField] SpriteRenderer sprite;
    

    public void Refresh()
    {
        collision.transform.localScale = new Vector3(sprite.size.x, sprite.size.y, 1) ;
    }
}
