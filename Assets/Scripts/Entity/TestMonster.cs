using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonster : Entity
{
    [Header("Reference")]
    [SerializeField] Animator animator;
    [SerializeField] Animator glowAnimator;

    [Header("Targeting")]
    [SerializeField] Vector2 target;
    [SerializeField] Vector2 playerLoc;
    [SerializeField] Vector2 lanternLoc;

    internal override void Start()
    {
        base.Start();
        Event_PlayerInRange += AttackNow;//Use Attack
    }

    internal override void Update()
    {
        base.Update();
        SelectDestination();
        AIMovementTo(target);
    }

    void AttackNow(object sender, TopDownCharacterController playerCont)
    {
        if (AttackTargetTime > Time.time) return;

        animator.SetTrigger("Attack");
        glowAnimator.SetTrigger("Attack");

        StartAttack(sender, playerCont);
    }
    void SelectDestination()
    {
        playerLoc = GameManager.instance.GetPlayerPosition();
        lanternLoc = GameManager.instance.GetLanternPosition();

        //if ((playerLoc - new Vector2(transform.position.x, transform.position.y)).magnitude < (lanternLoc - new Vector2(transform.position.x, transform.position.y)).magnitude)
        //{
        //    target = GameManager.instance.GetPlayerPosition();
        //}
        //else 
        if ((lanternLoc - new Vector2(transform.position.x, transform.position.y)).magnitude < (playerLoc - new Vector2(transform.position.x, transform.position.y)).magnitude)
        {
            target = GameManager.instance.GetLanternPosition();
        }
        else
        {
            target = GameManager.instance.GetPlayerPosition();
        }
    }
}