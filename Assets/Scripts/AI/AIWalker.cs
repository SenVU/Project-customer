using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]

public class AIWalker : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float airControlFactor = .1f;

    private Collider AICollider;
    private Nullable<Vector3> target = null;
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

    private void MoveTo(Nullable<Vector3> target)
    {
        if (!HasTarget()) return;
        Vector3 moveVect = target.Value - transform.position;
        moveVect.y = 0;
        moveVect.Normalize();

        // if the AI is grounded use a velosity based movement else use force based
        if (IsGrounded()) AIRigidbody.velocity = moveVect*speed;
        else AIRigidbody.AddForce(moveVect * airControlFactor);
        Debug.DrawLine(transform.position, target.Value, Color.yellow);
    }

    private void lookAt(Nullable<Vector3> LookTarget)
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
        Vector3 colliderBottom = new Vector3(AICollider.bounds.center.x, AICollider.bounds.min.y - .05f, AICollider.bounds.center.z);

        Vector3 checkCapsuleBottom = colliderBottom + new Vector3(0, 0.45f, 0);
        bool grounded = Physics.CheckCapsule(AICollider.bounds.center, checkCapsuleBottom, 0.45f);
        // old raycast based method
        //bool grounded = Physics.Linecast(playerCollider.bounds.center, colliderBottom);

        Debug.DrawLine(AICollider.bounds.center, colliderBottom, grounded ? Color.red : Color.blue);

        return grounded;
    }   

    public void SetTarget(Nullable<Vector3> target) { this.target = target; }
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
