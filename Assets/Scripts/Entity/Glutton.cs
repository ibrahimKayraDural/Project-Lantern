using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glutton : Entity
{
    [Header("Reference")]
    [SerializeField] Animator animator;
    [SerializeField] Animator glowAnimator;
    [SerializeField] LightColorType MonsterLightColor;
    [SerializeField] CircleCollider2D attackCollider;
    [SerializeField] LayerMask playerLayer;

    [Header("Values")]
    [SerializeField] float growthSpeed = 1;
    [SerializeField] float shrinkSpeed = 1;
    [SerializeField] float attackCooldown = 1;
    [SerializeField] float maxScale = 10;

    float nextAttack_TargetTime;
    float lastDeltaTime = .005f;
    bool inOrange;
    bool lightMatch;

    internal override void Start()
    {
        base.Start();

        entityLightType = MonsterLightColor;
    }

    internal override void Update()
    {
        base.Update();

        Collider2D col = Physics2D.OverlapCircle(transform.position, attackCollider.radius * transform.lossyScale.x, playerLayer);
        if (col != null)
        {
            if (col.gameObject.TryGetComponent(out TopDownCharacterController tdcc))
                Attack(tdcc);
        }

        lastDeltaTime = Time.deltaTime;

        if (inOrange == false)
            Grow(growthSpeed * lastDeltaTime);
        else
            Shrink(shrinkSpeed * lastDeltaTime);
    }

    void Shrink(float amount)
    {
        if (transform.localScale.x <= 0 || transform.localScale.y <= 0) Die();

        float bonusDamageValue = lightMatch ? damageBonus : 1;

        Vector3 targetScale = transform.localScale - new Vector3(1, 1, 0) * amount * bonusDamageValue;

        if(targetScale.x <= 0.1f || targetScale.y <= 0.1f)
        {
            Die();
        }
        else
        {
            transform.localScale = targetScale;
        }
    }
    void Grow(float amount)
    {
        if (transform.localScale.x >= maxScale || transform.localScale.y >= maxScale) return;

        transform.localScale = transform.localScale + new Vector3(1, 1, 0) * amount;
    }

    void Attack(TopDownCharacterController playerCont)
    {
        if (nextAttack_TargetTime > Time.time) return;

        nextAttack_TargetTime = Time.time + attackCooldown;

        Instantiate(attackObjectGO, playerCont.transform.position, Quaternion.identity);
    }

    public override void EnteredOrange(LightColorType lightColor)
    {
        lightMatch = lightColor == entityLightType;

        inOrange = true;
    }
    public override void ExitedOrange()
    {
        lightMatch = false;

        inOrange = false;
    }
    public override void RecieveDamage(float damage) { }
    public override void RecieveDamage(float damage, LightColorType lightColor) { }
    public override void IsInRed(float damage)
    {
        base.IsInRed(damage);
        Shrink(shrinkSpeed * lastDeltaTime);
    }
}
