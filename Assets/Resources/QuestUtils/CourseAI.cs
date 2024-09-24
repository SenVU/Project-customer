using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseAI : MonoBehaviour
{
    private bool isQuestStart = false;
    public Transform target;
    public Transform player;
    public float moveSpeed = 5f;
    public float stoppingDistance = 8f;
    public float detectionRange = 2f;
    public float playerAvoidDistance = 2f;
    public float avoidForce = 3f;
    public LayerMask obstacleLayer;
    public LayerMask groundLayer;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (isQuestStart && target != null && Vector3.Distance(transform.position, target.position) > stoppingDistance)
        {
            if (IsObstacleInFront())
            {
                AvoidObstacle();
            }
            else if (IsPlayerTooClose())
            {
                AvoidPlayer();
            }
            else
            {
                MoveTowardsTarget();
            }
        }

        AdjustHeightToGround();
    }

    public void StartQuest()
    {
        isQuestStart = true;
    }

    private void MoveTowardsTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
    }

    private bool IsObstacleInFront()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        return Physics.Raycast(ray, detectionRange, obstacleLayer);
    }

    private void AvoidObstacle()
    {
        Vector3 avoidDirection = transform.right;
        rb.MovePosition(transform.position + avoidDirection * avoidForce * Time.deltaTime);
    }

    private bool IsPlayerTooClose()
    {
        return player != null && Vector3.Distance(transform.position, player.position) < playerAvoidDistance;
    }

    private void AvoidPlayer()
    {
        Vector3 avoidDirection = (transform.position - player.position).normalized;
        rb.MovePosition(transform.position + avoidDirection * avoidForce * Time.deltaTime);
    }

    private void AdjustHeightToGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            Vector3 newPosition = transform.position;
            newPosition.y = hit.point.y + 1f;
            rb.MovePosition(newPosition);
        }
    }

    public int CheckIfSomeoneArrived()
    {
        float playerDistance = Vector3.Distance(player.position, target.position);
        float aiDistance = Vector3.Distance(transform.position, target.position);

        if (playerDistance <= stoppingDistance || aiDistance <= stoppingDistance)
        {
            return CheckWhoArrivesFirst();
        }
        return 2;
    }

    private int CheckWhoArrivesFirst()
    {
        float distanceToTargetAI = Vector3.Distance(transform.position, target.position);
        float distanceToTargetPlayer = Vector3.Distance(player.position, target.position);

        if (distanceToTargetAI < distanceToTargetPlayer)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}
