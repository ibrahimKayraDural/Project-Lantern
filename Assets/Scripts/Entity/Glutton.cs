using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glutton : Entity
{
    [Header("Reference")]
    [SerializeField] Animator animator;
    [SerializeField] Animator glowAnimator;
    [SerializeField] LightColorType MonsterLightColor;

    [SerializeField] GameObject magenta;

    [Header("Values")]
    [SerializeField] float growthSpeed = 1;
    [SerializeField] float attackCooldown = 1;
    [SerializeField] float maxScale = 10;

    float nextAttack_TargetTime;

    internal override void Start()
    {
        base.Start();

        entityLightType = MonsterLightColor;

        Event_PlayerInRange += Attack;
    }

    internal override void Update()
    {
        base.Update();

        Grow(growthSpeed * Time.deltaTime);
    }

    void Grow(float amount)
    {
        if (transform.localScale.x >= maxScale || transform.localScale.y >= maxScale) return;

        transform.localScale = transform.localScale + new Vector3(1, 1, 0) * amount;
    }

    void Attack(object sender, TopDownCharacterController playerCont)
    {
        if (nextAttack_TargetTime > Time.time) return;

        nextAttack_TargetTime = Time.time + attackCooldown;

        Instantiate(magenta, playerCont.transform.position, Quaternion.identity);
    }

    public override void EnteredOrange(LightColorType lightColor)
    {

    }
    public override void RecieveDamage(float damage) { }
    public override void RecieveDamage(float damage, LightColorType lightColor) { }
}
