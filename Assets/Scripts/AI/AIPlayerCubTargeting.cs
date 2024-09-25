using UnityEngine;

public class AIPlayerCubTargeting : AITargeter
{
    System.Random random = new System.Random();
    [SerializeField] State currentState = State.Idle;
    Vector2? wanderTarget;

    float idleTime;
    float chosenIdleTime;

    GameObject player;

    [SerializeField] float wanderSpeed = 2;
    [SerializeField] int maxWanderDistance = 10;
    [SerializeField] float wanderResetTargetDistance = .5f;
    [SerializeField] float maxDistanceFromPlayer = 20;
    [SerializeField] float minDistanceFromPlayer = 2;
    [SerializeField] float wanderToIdleChanse = .75f;

    [SerializeField] float minIdleTime = 1.5f;
    [SerializeField] float maxIdleTime = 5;


    [SerializeField] float followDistance = 5;

    [SerializeField] string playerObjectName = "Player";

    protected enum State
    {
        Wander,
        Follow,
        Idle,
        Disabled
    }

    protected override void Start()
    {
        player = GameObject.Find(playerObjectName);
        Debug.Assert(player != null, "Cub can't find player GameObject by name: (" + playerObjectName + ")");
        base.Start();
    }

    protected override void Update()
    {
        if (currentState == State.Disabled) return;

        if (Vec3ToVec2(transform.position - player.transform.position).magnitude > maxDistanceFromPlayer)
            currentState = State.Follow;

        if (currentState == State.Idle)
            Idle();
        if (currentState == State.Wander)
            Wander();
        if (currentState == State.Follow)
            Follow();

        if (maxDistanceFromPlayer > 0) Debug.DrawLine(transform.position, player.transform.position, Color.cyan);
    }

    protected void startIdle()
    {
        idleTime = 0;
        chosenIdleTime = minIdleTime + (maxIdleTime - minIdleTime) * ((float)random.NextDouble());
        currentState = State.Idle;
    }

    protected void Idle()
    {
        walker.SetTarget(null);
        wanderTarget = null;
        idleTime += Time.deltaTime;

        if (idleTime > maxIdleTime)
            currentState = State.Wander;
    }

    protected void Follow()
    {
        Vector2 playerPos = Vec3ToVec2(player.transform.position);
        Vector2 selfPos = GetVec2Pos();

        Vector2 direction = (selfPos - playerPos).normalized;

        walker.SetTargetVec2(playerPos + (direction * followDistance));

        if (Vec3ToVec2(transform.position - player.transform.position).magnitude < followDistance) currentState = State.Idle;
    }

    protected void Wander()
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
            while (!wanderTarget.HasValue && itterations < 11)
            {
                // if this loop has been called 10 times it is deemed a failure and it will target the starting point
                if (itterations > 10) wanderTarget = Vec3ToVec2(player.transform.position) + new Vector2(minDistanceFromPlayer, 0);
                else wanderTarget = GetVec2Pos() + RandomPosOffset();

                // check if the target is too far from the staring point and if so reset it
                if (maxDistanceFromPlayer > 0 && (wanderTarget.Value - Vec3ToVec2(player.transform.position)).magnitude > maxDistanceFromPlayer)
                    wanderTarget = null;
                else if (minDistanceFromPlayer > 0 && (wanderTarget.Value - Vec3ToVec2(player.transform.position)).magnitude < minDistanceFromPlayer)
                    wanderTarget = null;
                itterations++;
            }
        }
    }

    public void DeactivateAI() { if (currentState != State.Disabled) currentState = State.Disabled; }
    public void ActivateAI() { if (currentState == State.Disabled) startIdle(); }

    /// <summary>
    /// returns the Vec3 version of the target
    /// </summary>
    protected Vector3 Vec2ToVec3(Vector2 target) { return new Vector3(target.x, transform.position.y, target.y); }
    /// <summary>
    /// returns the Vec2 version of the target
    /// </summary>
    protected Vector2 Vec3ToVec2(Vector3 target) { return new Vector2(target.x, target.z); }
    /// <summary>
    /// gets the current position in the Vec2 format
    /// </summary>
    protected Vector2 GetVec2Pos() { return Vec3ToVec2(transform.position); }
    /// <summary>
    /// A Random Offset for finding a new target
    /// </summary>
    protected Vector2 RandomPosOffset() { return new Vector2(random.Next(-maxWanderDistance, maxWanderDistance), random.Next(-maxWanderDistance, maxWanderDistance)); }
}
