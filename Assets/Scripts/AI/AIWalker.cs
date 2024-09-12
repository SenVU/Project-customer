using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]

public class AIWalker : MonoBehaviour
{
    [SerializeField] float defaultSpeed;
    [SerializeField] float airControlFactor = .1f;
    float overrideSpeed;

    private Collider AICollider;
    private Vector3? target = null;
    private Rigidbody AIRigidbody;

    void Start()
    {
        AICollider = GetComponent<Collider>();
        AIRigidbody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// fixed update loop
    /// </summary>
    void FixedUpdate()
    {
        lookAt(target);
        MoveTo(target);
    }

    private void MoveTo(Vector3? target)
    {
        if (!HasTarget()) return;
        Vector3 moveVect = target.Value - transform.position;
        moveVect.y = 0;
        moveVect.Normalize();
        moveVect = moveVect * overrideSpeed;
        moveVect.y = AIRigidbody.velocity.y;
        // if the AI is grounded use a velosity based movement else use force based
        if (IsGrounded()) AIRigidbody.velocity = moveVect;
        else AIRigidbody.AddForce(moveVect * airControlFactor);
        Debug.DrawLine(transform.position, target.Value, Color.yellow);
    }

    private void lookAt(Vector3? LookTarget)
    {
        if (HasTarget()) transform.LookAt(LookTarget.Value);
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.x = 0;
        rotation.z = 0;
        transform.rotation = Quaternion.Euler(rotation);
    }

    /// <summary>
    /// checks if there is a collision whitin the distance of 0.01f below the AI
    /// </summary>
    public bool IsGrounded()
    {
        // needs a rework, the same thing as the player will not work 100%


        Vector3 colliderBottom = new Vector3(AICollider.bounds.center.x, AICollider.bounds.min.y - .05f, AICollider.bounds.center.z);

        Vector3 checkCapsuleBottom = colliderBottom + new Vector3(0, 0.45f, 0);
        bool grounded = Physics.CheckCapsule(AICollider.bounds.center, checkCapsuleBottom, 0.45f);
        // old raycast based method
        //bool grounded = Physics.Linecast(playerCollider.bounds.center, colliderBottom);

        Debug.DrawLine(AICollider.bounds.center, colliderBottom, grounded ? Color.red : Color.blue);

        return grounded;
    }   

    /// <summary>
    /// sets a new target
    /// </summary>
    /// <returns>true if the target is reacable</returns>
    public bool SetTarget(Vector3? target) 
    { 
        return SetTarget(target, defaultSpeed);
    }

    public bool SetTarget(Vector3? target, float overrideSpeed)
    {
        this.overrideSpeed = overrideSpeed;
        this.target = target;
        return true;
    }
    public bool HasTarget() { return target.HasValue; }

    public float GetDistanceToTarget()
    {
        return (target - transform.position).Value.magnitude;
    }

    public float GetHorizontalDistanceToTarget()
    {
        Vector3 horizontalTarget = target.Value;
        horizontalTarget.y = transform.position.y;
        return (horizontalTarget - transform.position).magnitude;
    }
}
