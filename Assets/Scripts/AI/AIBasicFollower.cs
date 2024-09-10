using UnityEngine;
using static UnityEngine.GraphicsBuffer;


[RequireComponent(typeof(AIWalker))]
public class AIBasicFollower : MonoBehaviour
{
    AIWalker walker;
    [SerializeField] GameObject followTarget;
    [SerializeField] float unsetTargetDistance;


    void Start()
    {
        Debug.Assert(followTarget != null, "FollowTarget is not set");
        walker = GetComponent<AIWalker>();
    }

    void Update()
    {
        if (!checkIfClose()) walker.SetTarget(followTarget.transform.position);
        else walker.SetTarget(null);
    }

    private bool checkIfClose()
    {
        return (transform.position - followTarget.transform.position).magnitude <= unsetTargetDistance)
        
    }
}
