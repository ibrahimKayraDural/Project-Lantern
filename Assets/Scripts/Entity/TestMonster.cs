using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonster : Entity
{
    [Header("Reference")]
    [SerializeField] Animator animator;
    [SerializeField] Animator glowAnimator;
    [SerializeField] LightColorType MonsterLightColor;

    [Header("Targeting")]
    [SerializeField] Vector2 target;
    [SerializeField] Vector2 playerLoc;
    [SerializeField] Vector2 lanternLoc;

    internal override void Start()
    {
        base.Start();

        entityLightType = MonsterLightColor;

        Event_PlayerInRange += AttackNow;//Use Attack

        target = transform.position;
    }

    internal override void Update()
    {
        base.Update();
        SelectDestination();
        //GameObject go = GameObject.FindGameObjectWithTag("Player");
        AIMovementTo(target);
    }

    void AttackNow(object sender, TopDownCharacterController playerCont)
    {
        if (AttackTargetTime > Time.time) return;

        animator.SetTrigger("Attack");

        if (glowAnimator != null) glowAnimator.SetTrigger("Attack");

        StartAttack(sender, playerCont);
    }
    void SelectDestination()
    {
        target = GameManager.instance.GetPlayerPosition();
    }
}