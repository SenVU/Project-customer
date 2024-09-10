using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerControler : MonoBehaviour
{
    private Rigidbody rigidBody;
    private Collider playerCollider;
    [SerializeField] private GameObject cam;
    Transform camTF;
    [SerializeField] private GameObject camTP;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float strafeSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float airControlFactor;

    [SerializeField] private float maxCamRotation;
    [SerializeField] private float minCamRotation;
    [SerializeField] private float camRotationSpeed;

    private float rotation = 0;
    private float camRotation = 0;
    private bool TPPov = false;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
        Debug.Assert(cam != null, "PlayerControler does not have a Camera attached");
        camTF =  cam.GetComponent<Transform>();
        Debug.Assert(camTP != null, "PlayerControler does not have a third person Camera attached");
        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
    }

    void FixedUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        float WS = Input.GetAxisRaw("Vertical");
        float AD = Input.GetAxisRaw("Horizontal");
        
        RotatePlayer(mouseX);
        RotateCamera(mouseY);

        MovePlayer(WS, AD);
        CheckForJump();
        CheckForCamSwitch();
    }

  

    private void RotateCamera(float mouseY)
    {
        if (cam.activeSelf)
        {
            camRotation -= mouseY * camRotationSpeed;
            camRotation = Mathf.Clamp(camRotation, minCamRotation, maxCamRotation);
        } else
        {
            camRotation = 0;
        }
        camTF.rotation = Quaternion.Euler(camRotation, rotation, 0);
    }

    private void RotatePlayer(float mouseX)
    {
        rotation = transform.rotation.eulerAngles.y;
        rotation += mouseX * rotationSpeed;
        transform.rotation = Quaternion.Euler(0, rotation, 0);
    }


    private void MovePlayer(float Forward, float AD)
    {
        Vector3 moveVect = new Vector3(AD * strafeSpeed, rigidBody.velocity.y, Forward * moveSpeed);
        moveVect = Quaternion.Euler(0, rotation, 0) * moveVect;


        if (IsGrounded()) rigidBody.velocity = moveVect;
        else rigidBody.AddForce(moveVect*airControlFactor);
    }

    /// <summary>
    /// checks for the jump key and jumps
    /// </summary>
    private void CheckForJump()
    {
        if (Input.GetKey(KeyCode.Space) && IsGrounded())
        {
            Vector3 jump = rigidBody.velocity;
            jump.y = jumpForce;
            rigidBody.velocity = jump;
        }
    }

    /// <summary>
    /// checks for the pressing of the key that switchest betwees 1st person and 3rd person
    /// </summary>
    private void CheckForCamSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) TPPov = !TPPov;
        cam.SetActive(!TPPov);
        camTP.SetActive(TPPov);
    }

    /// <summary>
    /// checks if there is a collision whitin the distance of 0.1f below the player
    /// </summary>
    public bool IsGrounded()
    {
        return Physics.CheckCapsule(playerCollider.bounds.center,
        new Vector3(playerCollider.bounds.center.x, playerCollider.bounds.min.y - 0.1f, playerCollider.bounds.center.z),
        0.45f);
        //return Physics.Raycast(transform.position+new Vector3(0,.1f,0), -Vector3.up, .2f);
    }
}
