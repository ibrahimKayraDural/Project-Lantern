using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(NavMeshAgent))]
public abstract class Entity : MonoBehaviour
{
    public enum MovementType { Normal, Locked}


    public MovementType currentMovementType { get; internal set; } = MovementType.Normal;
    public LightColorType entityLightType { get; internal set; } = LightColorType.Default;

    public event EventHandler<TopDownCharacterController> Event_PlayerInRange;

    public bool isDead { get; internal set; } = false;
    public bool isHinderable { get; internal set; } = true;
    
    public float FuelCostMulti => _fuelCostMultiplierInOrange;

    public string DebuffingName = "";

    [Header("STATS")]
    [SerializeField] internal float _speed = 5;
    [SerializeField] internal float _maxHealth = 5;
    [SerializeField] internal float _AITick = .1f;
    [SerializeField] internal float _fuelCostMultiplierInOrange = .1f;
    [SerializeField] internal float _speedInOrangePercent = 50;
    [SerializeField] internal float playerDetectRange = 1f;

    [Header("Ref")]
    [SerializeField] internal Transform MeshToFlip;
    [SerializeField] internal CircleCollider2D playerDetectCollider;
    [SerializeField] internal GameObject attackObjectGO;

    [Header("Values")]
    [SerializeField] internal float attackDuration = .5f;
    [SerializeField] internal float hinderBonus = 1.5f;
    [SerializeField] internal float damageBonus = 2f;

    internal float AttackTargetTime = float.MinValue;

    internal float _health = 1;
    public float MaxHealth { get => _maxHealth; set => _maxHealth = Mathf.Max(value, 1); }
    public float Health { get => _health; }
    public float Speed { get => _speed * speedModifierPercent / 100 * speedBonusPercent / 100; }

    Transform followingTransform;
    internal Rigidbody2D rb;
    internal NavMeshAgent agent;
    internal bool isLookingLeft;
    internal float moveTargetTime = 1;
    internal float speedModifierPercent = 100;
    internal float speedBonusPercent = 100;

    internal virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.drag = 0;
        rb.angularDrag = 0;
        rb.freezeRotation = true;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = Speed;

        playerDetectCollider.isTrigger = true;

        _health = _maxHealth;
    }
    internal virtual void Update()
    {
    }
    void OnDestroy()
    {
        agent.enabled = false;
    }
    void OnValidate()
    {
        playerDetectCollider.radius = playerDetectRange;
    }

    public virtual void IsInRed(float damage)
    {
        RecieveDamage(damage);
    }
    public virtual void RecieveDamage(float damage)
    {
        _health = Mathf.Max(0, _health - damage);

        if (_health == 0) Die();
    }
    public virtual void RecieveDamage(float damage, LightColorType lightColor)
    {
        float temp_bonusDamage = lightColor == entityLightType ? damageBonus : 1;

        _health = Mathf.Max(0, _health - damage * temp_bonusDamage);

        if (_health == 0) Die();
    }

    internal void StartAttack(object sender, TopDownCharacterController e)
    {
        if (AttackTargetTime > Time.time) return;

        LockMovementForSeconds(attackDuration);

        Vector2 playerPos = GameManager.instance.GetPlayerGO().transform.position;
        Vector2 attackVector = playerPos - (Vector2)transform.position;

        attackVector = attackVector.magnitude > playerDetectRange ? attackVector.normalized * playerDetectRange : attackVector;
        GameObject attackObject = Instantiate(attackObjectGO, (Vector2)transform.position + attackVector, Quaternion.identity);

        if (attackVector.x < 0)
        {
            Vector3 targetScale = attackObject.transform.localScale;
            targetScale.x = -targetScale.x;
            attackObject.transform.localScale = targetScale;
        }

        AttackTargetTime = Time.time + attackDuration;
    }

    #region oldMovement

    //IEnumerator MoveEnumeator;
    //internal void MoveTo(Vector2 here)
    //{
    //    if (isLookingLeft == false && transform.position.x > here.x)
    //        Flip();
    //    if (isLookingLeft && transform.position.x < here.x)
    //        Flip();

    //    if (MoveEnumeator != null)
    //        StopCoroutine(MoveEnumeator);

    //    MoveEnumeator = MoveToPoint(here, _AITick, movePrecision);
    //    StartCoroutine(MoveEnumeator);
    //}
    //internal void StopMoving()
    //{
    //    StopCoroutine(MoveEnumeator);
    //    rb.velocity = Vector2.zero;
    //}
    //IEnumerator MoveToPoint(Vector2 point, float intervalSeconds, float tolerance)
    //{
    //    int safety = 0;

    //    while (safety < 1000)
    //    {
    //        Vector2 pos = transform.position;

    //        if (Vector2.Distance(pos, point) <= tolerance) 
    //        {
    //            break;
    //        }

    //        Vector2 moveVector = (point - pos).normalized * _speed * intervalSeconds;

    //        if(rb.velocity != moveVector)
    //        {
    //            rb.velocity = moveVector;
    //        }

    //        yield return new WaitForSeconds(intervalSeconds);
    //        safety++;
    //    }

    //    StopMoving();
    //    yield break;
    //}

    #endregion

    internal void AIMovementTo(Vector2 targetPosition)
    {
        if (currentMovementType == MovementType.Locked) return;
        if (moveTargetTime > Time.time) return;

        if (isLookingLeft == false && transform.position.x > targetPosition.x)
            Flip();
        if (isLookingLeft && transform.position.x < targetPosition.x)
            Flip();

        targetPosition = new Vector3(targetPosition.x, targetPosition.y, 0);
        agent.SetDestination(targetPosition);

        moveTargetTime = Time.time + _AITick;
        rb.velocity = Vector2.zero;
    }

    internal virtual void Die() 
    { 
        isDead = true;
        //DieAnim

        agent.enabled = false;

        Destroy(gameObject);
    }
    void Flip()
    {
        if (MeshToFlip != null)
        {
            MeshToFlip.transform.localScale =
                new Vector3(-MeshToFlip.transform.localScale.x,
                MeshToFlip.transform.localScale.y,
                MeshToFlip.transform.localScale.z);
        }

        isLookingLeft = !isLookingLeft;
    }
    public void SetMovementType(MovementType setTo) => currentMovementType = setTo;


    internal delegate void del_MethodToInvoke(MovementType movementType);
    internal void LockMovementForSeconds(float seconds, del_MethodToInvoke methodToInvoke = null)
    {
        if (ChangeMovementTypeIEnum != null) StopCoroutine(ChangeMovementTypeIEnum);

        SetAIStopped(true);

        SetMovementType(MovementType.Locked);

        if (methodToInvoke == null)
            ChangeMovementTypeIEnum = SetMoveTypeAfterSeconds(MovementType.Normal, seconds);
        else
            ChangeMovementTypeIEnum = SetMoveTypeAfterSeconds(MovementType.Normal, seconds, methodToInvoke);

        StartCoroutine(ChangeMovementTypeIEnum);
    }
    IEnumerator ChangeMovementTypeIEnum;

    internal void SetAIStopped(bool value) => agent.isStopped = value;

    IEnumerator SetMoveTypeAfterSeconds(MovementType type, float seconds, del_MethodToInvoke methodToInvoke = null)
    {
        yield return new WaitForSeconds(seconds);

        switch (type)
        {
            case MovementType.Locked: SetAIStopped(true); break;

            case MovementType.Normal: SetAIStopped(false); break;
        }

        SetMovementType(type);
        ChangeMovementTypeIEnum = null;

        methodToInvoke?.Invoke(type);
    }

    public virtual void EnteredOrange(LightColorType lightColor)
    {
        HinderEntity(lightColor);
    }

    void HinderEntity(LightColorType lightColor)
    {
        if (AgentExists() && isHinderable)
        {
            float temp_bonusHinder = lightColor == entityLightType ? hinderBonus : 1;

            speedModifierPercent = _speedInOrangePercent / temp_bonusHinder;
            agent.speed = Speed;
        }
        //SlowedWalkAnim
    }

    public virtual void ExitedOrange()
    {
        SetSpeedModifierToDefault();
    }

    void SetSpeedModifierToDefault()
    {
        if (AgentExists())
        {
            speedModifierPercent = 100;
            agent.speed = Speed;
        }
        //NormalWalkAnim
    }

    public void SetSpeedBonusPercent(float value)
    {
        if (AgentExists())
        {
            speedBonusPercent = value;
            agent.speed = Speed;
        }
    }
    public void SetSpeedBonusToDefault()
    {
        if (AgentExists())
        {
            speedBonusPercent = 100;
            agent.speed = Speed;
        }
    }

    bool AgentExists()
    {
        return agent != null;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(collision.gameObject.TryGetComponent(out TopDownCharacterController playerController))
            {
                Event_PlayerInRange?.Invoke(this, playerController);
            }
        }
    }
}
