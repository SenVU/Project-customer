using UnityEngine;


[RequireComponent(typeof(AIWalker))]
public class AIBasicFollower : MonoBehaviour
{
    AIWalker walker;
    [SerializeField] GameObject followTarget;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(followTarget != null, "FollowTarget is not set");
        walker = GetComponent<AIWalker>();
    }

    // Update is called once per frame
    void Update()
    {
        walker.SetTarget(followTarget.transform.position);
    }
}
