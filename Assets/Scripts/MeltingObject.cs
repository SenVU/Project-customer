using UnityEngine;

public class MeltingObject : MonoBehaviour
{

    Rigidbody rigidBody;
    [SerializeField] private float meltSpeed;
    [SerializeField] private bool meltOnSpawn;
    [SerializeField] private bool destroyAfterMelt = true;
    private bool isMelting;

    /// <summary>
    /// setup on spawn
    /// </summary>
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        SetMelting(meltOnSpawn);
    }

    /// <summary>
    /// fixed updateloop
    /// </summary>
    void FixedUpdate()
    {
        if (isMelting)
        {
            transform.localScale = transform.localScale - (meltSpeed * Vector3.one);
        }
        // check if the objects size is 0
        if (transform.localScale.x <= 0 || transform.localScale.y <= 0 || transform.localScale.z <= 0)
        {
            transform.localScale= Vector3.zero;
            if (destroyAfterMelt)
            {
                // destroy the object
                Destroy(gameObject);
                return;
            }
            // set melting to false to stop the process just in case
            isMelting = false;
        }
        
    }

    public void StartMelting() { isMelting = true; }
    public void StopMelting() { isMelting = false; }
    public void SetMelting(bool state) { isMelting = state; }
    public bool IsMelting() { return isMelting; }
}
