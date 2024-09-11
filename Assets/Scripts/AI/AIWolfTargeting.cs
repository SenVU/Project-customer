using HoudiniEngineUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWolfTargeting : AITargeter
{
    List<GameObject> potentialTargets;
    System.Random random = new System.Random();
    State currentState = State.Wander;
    Vector2? wanderTarget;

    [SerializeField] int maxWanderDistance = 15;
    [SerializeField] float wanderResetTargetDistance = 2;

    enum State
    {
        Wander,
        Attack,
        Idle
    }

    protected override void Update()
    {
        if (currentState == State.Idle) return;
        if (currentState == State.Attack)
        {
            if (followTarget != null)
            {
                walker.SetTarget(followTarget.transform.position);
            }
            else ChooseNewTarget();
        }
        if (currentState == State.Wander) 
        {
            Wander();
        }
    }

    public void Wander()
    {
        if (wanderTarget.HasValue)
        {
            walker.SetTarget(WanderTargetToVec3(wanderTarget.Value));
            if (walker.GetHorizontalDistanceToTarget() < wanderResetTargetDistance) wanderTarget = null;
        }
        else 
        {
            wanderTarget = GetVec2Pos() + RandomPosOffset();
        }
    }

    void findTargets() { }

    public void ChooseNewTarget()
    {
        if (potentialTargets.Count>0)
        {
            followTarget = potentialTargets[random.Next(potentialTargets.Count)];
        }
    }

    Vector3 WanderTargetToVec3(Vector2 wanderTarget)
    {
        return new Vector3(wanderTarget.x, transform.position.y, wanderTarget.y);
    }

    Vector2 GetVec2Pos() { return new Vector2(transform.position.x, transform.position.z); }
    Vector2 RandomPosOffset() { return new Vector2(random.Next(-maxWanderDistance, maxWanderDistance), random.Next(-maxWanderDistance, maxWanderDistance)); }
}
