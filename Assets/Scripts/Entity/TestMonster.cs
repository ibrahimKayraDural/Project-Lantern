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
        SelectDestination();
        AIMovementTo(target);
        Debug.LogWarning(lanternLoc);

    }

    void SelectDestination()
    {
        playerLoc = GameManager.instance.GetPlayerPosition();
        lanternLoc = GameManager.instance.GetLanternPosition();



        if ((playerLoc - new Vector2(transform.position.x, transform.position.y)).magnitude < (lanternLoc - new Vector2(transform.position.x, transform.position.y)).magnitude)
        {
            target = GameManager.instance.GetPlayerPosition();
            Debug.LogError("PLAYER");
        }
        else if ((lanternLoc - new Vector2(transform.position.x, transform.position.y)).magnitude < (playerLoc - new Vector2(transform.position.x, transform.position.y)).magnitude)
        {
            target = GameManager.instance.GetLanternPosition();
            Debug.LogError("LANTERN");

        }
        else
        {
            //Debug.Log("What");
        }

    }
}
