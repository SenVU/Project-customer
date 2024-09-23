using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerControler : MonoBehaviour
{
    private Rigidbody rigidBody;
    private Collider playerCollider;
    [Header("Objects")]
    Transform FPPCamTF;
    [SerializeField] private GameObject FPPCam;
    [SerializeField] private GameObject TPPCam;

    [Header("Controll Forces")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float strafeSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float airControlFactor;

    [Header("FPP camera")]
    [SerializeField] private float maxCamRotation;
    [SerializeField] private float minCamRotation;
    [SerializeField] private float camRotationSpeed;

    [Header("Keybinds")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode camSwitchKey = KeyCode.Tab;

    private float playerYawRotation = 0;
    private float camPitchRotation = 0;

    /// <summary>
    /// setup at player spawn
    /// </summary>
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
        Debug.Assert(playerCollider != null, "Players Collider not found");
        Debug.Assert(FPPCam != null, "PlayerControler does not have a Camera attached");
        FPPCamTF =  FPPCam.GetComponent<Transform>();
        Debug.Assert(TPPCam != null, "PlayerControler does not have a third person Camera attached");
        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
    }

    /// <summary>
    /// fixed update loop
    /// </summary>
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


    /// <summary>
    /// camera rotation control
    /// </summary>
    /// <param name="mouseY">mouse Y axis</param>
    private void RotateCamera(float mouseY)
    {
        // checks if the FP camera is active
        if (FPPCam.activeSelf)
        {
            camPitchRotation -= mouseY * camRotationSpeed;
            camPitchRotation = Mathf.Clamp(camPitchRotation, minCamRotation, maxCamRotation);
        } else
        {
            camPitchRotation = 0;
        }
        FPPCamTF.rotation = Quaternion.Euler(camPitchRotation, playerYawRotation, 0);
    }

    /// <summary>
    /// player mouse rotation control
    /// </summary>
    /// <param name="mouseX">mouse X axis</param>
    private void RotatePlayer(float mouseX)
    {
        playerYawRotation = transform.rotation.eulerAngles.y;
        playerYawRotation += mouseX * rotationSpeed;
        transform.rotation = Quaternion.Euler(0, playerYawRotation, 0);
    }

    /// <summary>
    /// player movement control
    /// </summary>
    /// <param name="Forward">the forward input</param>
    /// <param name="Strafe">the sideways input</param>
    private void MovePlayer(float Forward, float Strafe)
    {
        Vector3 moveVect = new Vector3(Strafe * strafeSpeed, rigidBody.velocity.y, Forward * moveSpeed);
        moveVect = Quaternion.Euler(0, playerYawRotation, 0) * moveVect;

        // if the player is grounded use a velosity based movement else use force based
        if (IsGrounded()) rigidBody.velocity = moveVect;
        else rigidBody.AddForce(moveVect*airControlFactor);
    }

    /// <summary>
    /// checks for the jump key and jumps
    /// </summary>
    private void CheckForJump()
    {
        if (Input.GetKey(jumpKey) && IsGrounded())
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
        if (Input.GetKeyDown(camSwitchKey))
        {
            FPPCam.SetActive(!FPPCam.activeSelf);
            TPPCam.SetActive(!FPPCam.activeSelf);
        }
    }

    /// <summary>
    /// checks if there is a collision whitin the distance of 0.01f below the player
    /// </summary>
    public bool IsGrounded()
    {
        Vector3 colliderBottom = new Vector3(playerCollider.bounds.center.x, playerCollider.bounds.min.y , playerCollider.bounds.center.z);
        float sphereRadius = 1.01f;
        Vector3 checkSperePoint = colliderBottom + new Vector3(0, sphereRadius, 0);
        bool grounded = Physics.CheckSphere(checkSperePoint, sphereRadius);
        // old raycast based method
        //bool grounded = Physics.Linecast(playerCollider.bounds.center, colliderBottom);

        Debug.DrawLine(checkSperePoint, checkSperePoint + Vector3.down*sphereRadius, grounded ? Color.red : Color.blue);

        return grounded;
    }
}
