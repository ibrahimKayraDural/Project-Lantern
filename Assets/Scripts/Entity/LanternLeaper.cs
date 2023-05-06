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
    private void Start()
    {
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
        }

        if ((target - new Vector2(transform.position.x, transform.position.y)).magnitude > LeapRange && _state != State.Ready && _state != State.Attack)
        {
            _state = State.Chase;
        }

        if ((target - new Vector2(transform.position.x, transform.position.y)).magnitude <= LeapRange && _state != State.Attack)
        {
            _state = State.Ready;

             leapTime += Time.deltaTime;
        }

        if (LeapAnticipation <= Time.deltaTime)
        {
            _state = State.Attack;
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
