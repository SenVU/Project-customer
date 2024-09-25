using UnityEngine;


[RequireComponent(typeof(AIWalker))]
public class AITargeter : MonoBehaviour
{
    protected AIWalker walker;


    virtual protected void Start()
    {
        walker = GetComponent<AIWalker>();
    }

    virtual protected void Update()
    {
        
    }

    
}
