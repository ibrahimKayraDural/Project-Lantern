using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternLeaper : Entity
{
    [Header("Targeting")]
    [SerializeField] Vector2 target;

    [Header("LanternLeaperStats")]
    [SerializeField] float LeapRange;
    [SerializeField] float LeapForce;
    [SerializeField] float LeapAnticipation;

    float leapTime;

    State _state = State.Chase;
    internal override void Start()
    {
        base.Start();
        leapTime = LeapAnticipation;
    }

    internal override void Update()
    {
        base.Update();
        SelectDestination();
        

        switch (_state)
        {
            case State.Chase:
                AIMovementTo(target);
                break;
            case State.Ready:
                break;
            case State.Attack:
                break;
            case State.Hurt:
                break;
            case State.Dead:
                break;
        }

        if ((target - new Vector2(transform.position.x, transform.position.y)).magnitude > LeapRange /*&& _state != State.Ready && _state != State.Attack && _state != State.Hurt && _state != State.Dead*/)
        {
            _state = State.Chase;
        }

        if ((target - new Vector2(transform.position.x, transform.position.y)).magnitude <= LeapRange /*&& _state != State.Attack*/)
        {
            _state = State.Ready;

             leapTime += Time.time;
        }

        if (leapTime <= Time.time && leapTime != LeapAnticipation && rb.velocity.magnitude < 1)
        {
            _state = State.Attack;
            rb.velocity = target - new Vector2(transform.position.x, transform.position.y).normalized * LeapForce;
            leapTime = LeapAnticipation;
        }

        if (rb.velocity.magnitude > 1)
        {
            _state = State.Chase;
        }
        

    }

    enum State 
    {
        Chase,
        Ready,
        Attack,
        Hurt,
        Dead

    }
    void SelectDestination()
    {
        target = GameManager.instance.GetLanternPosition();
    }
}
