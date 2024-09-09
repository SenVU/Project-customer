using System;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    Rigidbody rigidBody;
    [SerializeField] GameObject cam;

    [SerializeField] float moveSpeed;
    [SerializeField] float strafeSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float airControlFactor;

    [SerializeField] float maxCamRotation;
    [SerializeField] float minCamRotation;
    [SerializeField] float camRotationSpeed;

    float rotation = 0;
    float camRotation = 0;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        Debug.Assert(cam != null, "PlayerControler does not have a Camera attached");
        Application.targetFrameRate = 60;
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
    }

  

    private void RotateCamera(float mouseY)
    {
        Transform camTF = cam.GetComponent<Transform>();
        camRotation -= mouseY*camRotationSpeed;
        camRotation = Mathf.Clamp(camRotation, minCamRotation, maxCamRotation);
        camTF.rotation = Quaternion.Euler(camRotation, rotation, 0);
    }

    private void RotatePlayer(float mouseX)
    {
        rotation = transform.rotation.eulerAngles.y;
        rotation += mouseX * rotationSpeed;
        transform.rotation = Quaternion.Euler(0, rotation, 0);
    }

    private void MovePlayer(float WS, float AD)
    {
        Vector3 moveVect = new Vector3(AD * strafeSpeed, 0, WS * moveSpeed);
        moveVect = Quaternion.Euler(0, rotation, 0) * moveVect;


        if (IsGrounded()) rigidBody.velocity = moveVect;
        else rigidBody.AddForce(moveVect*airControlFactor);
    }

    private void CheckForJump()
    {
        if (Input.GetKey(KeyCode.Space) && IsGrounded())
        {
            Vector3 jump = rigidBody.velocity;
            jump.y = jumpForce;
            rigidBody.velocity = jump;
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position+new Vector3(0,.1f,0), -Vector3.up, .2f);
    }
}
