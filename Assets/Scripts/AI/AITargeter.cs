using UnityEngine;
using static UnityEngine.GraphicsBuffer;


[RequireComponent(typeof(AIWalker))]
public class AITargeter : MonoBehaviour
{
    protected AIWalker walker;
    [SerializeField] protected GameObject followTarget;


    virtual protected void Start()
    {
        walker = GetComponent<AIWalker>();
    }

    virtual protected void Update()
    {
        walker.SetTarget(followTarget.transform.position);
    }

    
}
