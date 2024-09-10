using System;
using UnityEngine;


[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]

public class AIWalker : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float airControlFactor = .1f;
    [SerializeField] float unsetTargetDistance;

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
        checkIfClose();
        lookAt(target);
        MoveTo(target);
    }

    private void MoveTo(Nullable<Vector3> target)
    {
        if (!HasTarget()) return;
        Vector3 moveVect = target.Value - transform.position;
        moveVect.y = 0;

        // if the AI is grounded use a velosity based movement else use force based
        if (IsGrounded()) AIRigidbody.velocity = moveVect;
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

    private void checkIfClose()
    {
        if (HasTarget() && (transform.position-target.Value).magnitude<=unsetTargetDistance)
        {
            target = null;
        }
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
}
