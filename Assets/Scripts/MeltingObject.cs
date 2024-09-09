using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeltingObject : MonoBehaviour
{

    Rigidbody rigidBody;
    [SerializeField] private float meltSpeed;
    [SerializeField] private bool meltOnSpawn;
    [SerializeField] private bool destroyAfterMelt = true;
    private bool isMelting;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        isMelting = meltOnSpawn;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.localScale.x <= 0 || transform.localScale.y <= 0 || transform.localScale.z <= 0)
        {
            if (destroyAfterMelt) Destroy(gameObject);
            return;
        }
        if (isMelting)
        {
            transform.localScale = transform.localScale - (meltSpeed * Vector3.one);
        }
    }

    public void startMelting() { isMelting = true; }
    public void stopMelting() { isMelting = false; }
    public void setMelting(bool state) { isMelting = state; }
    public bool getIsMelting() { return isMelting; }
}
