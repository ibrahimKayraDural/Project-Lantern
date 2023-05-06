using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(NavMeshAgent))]
public abstract class Entity : MonoBehaviour
{
    public bool isDead { get; private set; } = false;
    public float FuelCostMulti => _fuelCostMultiplierInOrange;

    public string DebuffingName = "";

    [Header("STATS")]
    [SerializeField] internal float _speed = 5;
    [SerializeField] internal float _maxHealth = 5;
    [SerializeField] internal float _AITick = .1f;
    [SerializeField] internal float _fuelCostMultiplierInOrange = .1f;
    [SerializeField] internal float _speedInOrangePercent = 50;

    [Header("Ref")]
    [SerializeField] internal Transform MeshToFlip;

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

    void Start()
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

        _health = _maxHealth;
    }
    internal virtual void Update()
    {

    }

    public void RecieveDamage(int damage)
    {
        _health = Mathf.Max(0, _health - damage);

        if (_health == 0) Die();
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
        if (moveTargetTime > Time.time)
            return;

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

    public void EnteredOrange()
    {
        speedModifierPercent = _speedInOrangePercent;
        agent.speed = Speed;
    }
    public void SetSpeedToDefault()
    {
        speedModifierPercent = 100;
        agent.speed = Speed;
    }
}
