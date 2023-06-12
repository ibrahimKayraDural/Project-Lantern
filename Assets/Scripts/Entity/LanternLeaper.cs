using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternLeaper : Entity
{
    public enum State
    {
        Chase,
        Charging,
        Attack,
        Hurt,
        Dead
    }

    [Header("Reference")]
    [SerializeField] LayerMask lanternLayer;

    [Header("LanternLeaperStats")]
    [SerializeField] float FuelCostDamage = 2.5f;
    [SerializeField] float LeapRange = 2;
    [SerializeField] float LeapDuration = 1;
    [SerializeField] float LeapSpeedBonusPercent = 500;
    [SerializeField] float LeapAnticipation = 2.5f;
    [SerializeField] float LeapDamagingArea = 1f;
    
    Vector2 target;
    State _state = State.Chase;

    float TargetTime_attackStart = float.MaxValue;
    float TargetTime_attackEnd = float.MinValue;
    bool isCharging;
    bool hasDamagedThisLeap;
    Vector3 leapDirection;

    internal override void Start()
    {
        base.Start();
    }
    internal override void Update()
    {
        base.Update();
        SelectDestination();

        if (Time.time >= TargetTime_attackStart)
        {
            _state = State.Attack;
            TargetTime_attackStart = float.MaxValue;
        }

        switch (_state)
        {
            case State.Chase:

                AIMovementTo(target);

                if (DistanceWithTarget() <= LeapRange)
                {
                    _state = State.Charging;
                }

                break;

            case State.Charging:

                if (isCharging) break;

                isCharging = true;

                LockMovementForSeconds(LeapAnticipation, (MovementType mType) => _state = State.Attack);

                break;

            case State.Attack:

                if (isCharging)
                {
                    isCharging = false;

                    leapDirection = target - (Vector2)transform.position;
                    SetSpeedBonusPercent(LeapSpeedBonusPercent);

                    isHinderable = false;
                    TargetTime_attackEnd = Time.time + LeapDuration;
                }

                //Vector2 rayVector = target - (Vector2)transform.position;
                //RaycastHit2D hit = Physics2D.Raycast(transform.position, rayVector.normalized, LeapTime, 1 << 0);// 1 << 0 == defaultLayer 
                //Vector3 targetVector = hit.collider == null ? target : hit.point;

                Vector3 targetVector = transform.position + leapDirection;
                AIMovementTo(targetVector);
                TryDamageLantern();

                if (Time.time >= TargetTime_attackEnd)
                {
                    SetSpeedBonusToDefault();
                    isHinderable = true;
                    hasDamagedThisLeap = false;

                    _state = State.Chase;
                }

                break;

            case State.Hurt:
                break;

            case State.Dead:
                break;
        }
    }

    bool TryDamageLantern()
    {
        if (hasDamagedThisLeap) return false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, LeapDamagingArea, lanternLayer);
        
        foreach(Collider2D col in colliders)
        {
            if (col.TryGetComponent(out FuelController fc))
            {
                if (col != fc.MeshCollider) continue;

                fc.RemoveFuel(FuelCostDamage);
                hasDamagedThisLeap = true;

                Debug.Log("Damaged " + col.gameObject.name);

                return true;
            }
        }

        return false;
    }
    void SelectDestination()
    {
        target = GameManager.instance.GetLanternPosition();
    }
    float DistanceWithTarget() => Vector3.Distance(transform.position, target);

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, LeapRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, LeapDamagingArea);
    }
}
