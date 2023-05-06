using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonster : Entity
{
    [SerializeField] Vector2 target;
    [SerializeField] Vector2 playerLoc;
    [SerializeField] Vector2 lanternLoc;

    internal override void Update()
    {
        base.Update();
        if (target == null)
        {
            SelectDestination();
        }
        AIMovementTo(target);
    }

    void SelectDestination()
    {
        playerLoc = GameManager.instance.GetPlayerPosition();
        lanternLoc = GameManager.instance.GetLanternPosition();



        if ((playerLoc - new Vector2(transform.position.x, transform.position.y)).magnitude < (lanternLoc - new Vector2(transform.position.x, transform.position.y)).magnitude)
        {
            target = GameManager.instance.GetPlayerPosition();
        }
        else if ((lanternLoc - new Vector2(transform.position.x, transform.position.y)).magnitude < (lanternLoc - new Vector2(transform.position.x, transform.position.y)).magnitude)
        {
            target = GameManager.instance.GetLanternPosition();
        }

    }
}
