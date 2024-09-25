using System.Collections.Generic;
using UnityEngine;

public class AIWolfTargeting : AITargeter
{
    List<GameObject> potentialTargets;
    System.Random random = new System.Random();
    State currentState = State.Wander;
    Vector2? wanderTarget;
    Vector2 startPoint;
    GameObject attackTarget;

    float idleTime;
    float chosenIdleTime;

    float lastAttackTime;


    [SerializeField] float wanderSpeed = 2;
    [SerializeField] int maxWanderDistance = 15;
    [SerializeField] float wanderResetTargetDistance = .5f;
    /// <summary>
    /// leave 0 for infinite
    /// </summary>
    [SerializeField] float maxDistanceFromSpawn;
    [SerializeField] float wanderToIdleChanse = .75f;

    [SerializeField] float minIdleTime = 1.5f;
    [SerializeField] float maxIdleTime = 5;


    [SerializeField] float maxTargetSearchDistance=10;
    [SerializeField] float maxTargetTrackDistance=15;

    [SerializeField] float attackWalkSpeed = 4;

    [SerializeField] float attackDistance = .75f;
    [SerializeField] float attackDamage = 1f;
    [SerializeField] float attackInterval = 1f;

    enum State
    {
        Wander,
        Attack,
        Idle
    }

    protected override void Start()
    {
        startPoint = GetVec2Pos();
        base.Start();
    }

    protected override void Update()
    {
        FindAttackTargets();

        // if there are targets switch to attack mode
        if (potentialTargets.Count > 0) 
            currentState = State.Attack;

        if (currentState == State.Idle)
            Idle();
        if (currentState == State.Attack)
            Attack();
        if (currentState == State.Wander) 
            Wander();
        

        if (maxDistanceFromSpawn > 0) Debug.DrawLine(transform.position, Vec2ToVec3(startPoint), Color.cyan);
    }

    public void startIdle()
    {
        idleTime = 0;
        chosenIdleTime = minIdleTime + (maxIdleTime-minIdleTime)*((float)random.NextDouble());
        currentState = State.Idle;
    }

    public void Idle()
    {
        walker.SetTarget(null);
        wanderTarget=null;
        idleTime += Time.deltaTime;
        if (idleTime > maxIdleTime) 
            currentState = State.Wander;
    }

    public void Wander()
    {
        if (wanderTarget.HasValue)
        {
            // sets a new target, if the walker deems it unreachable the target is unset
            if (!walker.SetTargetVec2(wanderTarget.Value, wanderSpeed))
                wanderTarget = null;

            // unset the target if it has been reached
            if (walker.HasTarget() && walker.GetHorizontalDistanceToTarget() < wanderResetTargetDistance)
            {
                wanderTarget = null;
                // random chanse to go Idle
                if (random.NextDouble() < wanderToIdleChanse) { startIdle(); }
            }
        }
        else 
        {
            int itterations = 0;
            while (!wanderTarget.HasValue)
            {
                // if this loop has been called 10 times it is deemed a failure and it will target the starting point
                if (itterations > 10) wanderTarget = startPoint;
                else wanderTarget = GetVec2Pos() + RandomPosOffset();

                // check if the target is too far from the staring point and if so reset it
                if (maxDistanceFromSpawn>0 && (wanderTarget.Value - startPoint).magnitude > maxDistanceFromSpawn) 
                    wanderTarget = null;
                itterations++;
            }
        }
    }

    void Attack()
    {
        if (attackTarget != null)
        {
            if ((transform.position - attackTarget.transform.position).magnitude > maxTargetTrackDistance)
                attackTarget = null;
            else walker.SetTarget(attackTarget.transform.position, attackWalkSpeed);
            if (lastAttackTime >= attackInterval)
            {
                if ((transform.position - attackTarget.transform.position).magnitude <= attackDistance)
                {
                    lastAttackTime = 0;
                    attackTarget.GetComponent<HealthManager>().Damage(attackDamage, HealthManager.DamageSource.Wolf);
                }
            }
            else lastAttackTime += Time.deltaTime;
        }
        else ChooseNewTarget();
    }

    void FindAttackTargets()
    {
        potentialTargets = new List<GameObject>();
        Collider[] foundColliders = Physics.OverlapSphere(transform.position, maxTargetSearchDistance);
        foreach(Collider collider in foundColliders)
        {
            GameObject obj = collider.gameObject;
            if (isAttackable(obj) && !potentialTargets.Contains(obj))
            {
                potentialTargets.Add(obj);
            }
        }
    }

    bool isAttackable(GameObject obj)
    {
        bool toReturn = true;
        toReturn = toReturn && obj!=gameObject;
        toReturn = toReturn && (obj.GetComponent<HealthManager>() != null);
        toReturn = toReturn && (obj.CompareTag("Player") || obj.CompareTag("PlayerCub"));
        return toReturn;
    }

    public void ChooseNewTarget()
    {
        if (potentialTargets.Count > 0)
        {
            attackTarget = potentialTargets[random.Next(potentialTargets.Count)];
        }
        else startIdle();
    }

    /// <summary>
    /// returns the Vec3 version of the target
    /// </summary>
    Vector3 Vec2ToVec3(Vector2 target) { return new Vector3(target.x, transform.position.y, target.y); }
    /// <summary>
    /// gets the current position in the Vec2 format
    /// </summary>
    Vector2 GetVec2Pos() { return new Vector2(transform.position.x, transform.position.z); }
    /// <summary>
    /// A Random Offset for finding a new target
    /// </summary>
    Vector2 RandomPosOffset() { return new Vector2(random.Next(-maxWanderDistance, maxWanderDistance), random.Next(-maxWanderDistance, maxWanderDistance)); }
}
