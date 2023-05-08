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



    public MovementType currentMovementType { get; private set; } = MovementType.Normal;

    public event EventHandler<TopDownCharacterController> Event_PlayerInRange;

    public bool isDead { get; private set; } = false;
    
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

    internal float AttackTargetTime = float.MinValue;

    internal float _health = 1;
    public float MaxHealth { get => _maxHealth; set => _maxHealth = Mathf.Max(value, 1); }
    public float Health { get => _health; }
    public float Speed { get => _speed * speedModifierPercent / 100; }

    Transform followingTransform;
    internal Rigidbody2D rb;
    internal NavMeshAgent agent;
    internal bool isLookingLeft;
    internal float moveTargetTime = -1;
    internal float speedModifierPercent = 100;


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

    public void RecieveDamage(float damage)
    {
        _health = Mathf.Max(0, _health - damage);

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
    }
    void Die() 
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

    public void LockMovementForSeconds(float seconds)
    {
        if (ChangeMovementTypeIEnum != null) StopCoroutine(ChangeMovementTypeIEnum);

        SetMovementType(MovementType.Locked);
        ChangeMovementTypeIEnum = SetMoveTypeAfterSeconds(MovementType.Normal, seconds);
        StartCoroutine(ChangeMovementTypeIEnum);
    }
    IEnumerator ChangeMovementTypeIEnum;
    IEnumerator SetMoveTypeAfterSeconds(MovementType type, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SetMovementType(type);
        ChangeMovementTypeIEnum = null;
    }
    public void EnteredOrange()
    {
        speedModifierPercent = _speedInOrangePercent;
        if (AgentExists()) agent.speed = Speed;
        //SlowedWalkAnim
    }
    public void SetSpeedToDefault()
    {
        speedModifierPercent = 100;
        if (AgentExists()) agent.speed = Speed;
        //NormalWalkAnim
    }
    bool AgentExists() => agent != null;

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
