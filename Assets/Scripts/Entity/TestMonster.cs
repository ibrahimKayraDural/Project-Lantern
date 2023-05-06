using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonster : Entity
{
    internal override void Update()
    {
        base.Update();

        AIMovementTo(GameManager.instance.GetPlayerPosition());
    }
}
