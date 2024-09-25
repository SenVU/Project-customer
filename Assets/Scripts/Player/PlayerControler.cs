using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerControler : MonoBehaviour
{
    private Vector3 FPPCamLocalPos;
    private Rigidbody rigidBody;
    private Collider playerCollider;
    [Header("Objects")]
    //Transform FPPCamTF;
    [SerializeField] private GameObject cam;

    [Header("Controll Forces")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float runSpeed;
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
    [SerializeField] private KeyCode runKey = KeyCode.LeftShift;

    [SerializeField] private float swimControlFactor = 5f;
    [SerializeField] private float swimHeight = -.5f;

    private float playerYawRotation = 0;
    private float camPitchRotation = 0;


    [Header("Camera")]
    private bool isTPP;
    private Transform camTransform;

    [SerializeField] private float TPPDistanceAway = 4f;
    [SerializeField] private float TPPDistanceUp = 1.8f;

    private Vector3 velocityCanSmooth = Vector3.zero;
    [SerializeField] private float TPPCanSmoothDampTime = .15f;

    private Vector3 TPPLookDir;
    private Vector3 TPPTargetPos;

    private bool camSwiched = false;

    [Header("Animation")]

    [SerializeField] private Animator animator;
    private bool eating;


    /// <summary>
    /// setup at player spawn
    /// </summary>
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
        Debug.Assert(playerCollider != null, "Players Collider not found");
        Debug.Assert(cam != null, "PlayerControler does not have a Camera attached");
        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        camTransform = cam.transform;

        FPPCamLocalPos = camTransform.localPosition;
    }

    /// <summary>
    /// update loop
    /// </summary>
    private void Update()
    {
        CheckForCamSwitch();
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
        bool running = Input.GetKey(runKey);

        if (isTPP) TPPCamUpdate();
        else FPPCamUpdate(mouseY);

        RotatePlayer(mouseX);

        MovePlayer(WS, AD, running);
        CheckForJump();
            
        FloatInWater();

        HandleAnimation(WS, running);
    }


    /// <summary>
    /// first person camera update tick
    /// </summary>
    /// <param name="mouseY">mouse Y axis</param>
    private void FPPCamUpdate(float mouseY)
    {
        camTransform.localPosition = FPPCamLocalPos;
        // checks if the FP camera is active
        if (cam.activeSelf)
        {
            camPitchRotation -= mouseY * camRotationSpeed;
            camPitchRotation = Mathf.Clamp(camPitchRotation, minCamRotation, maxCamRotation);
        }
        else
        {
            camPitchRotation = 0;
        }
        camTransform.rotation = Quaternion.Euler(camPitchRotation, playerYawRotation, 0);
    }

    /// <summary>
    /// player mouse rotation control
    /// </summary>
    /// <param name="mouseX">mouse X axis</param>
    private void RotatePlayer(float mouseX)
    {
        if (eating) { return; }
        playerYawRotation = transform.rotation.eulerAngles.y;
        playerYawRotation += mouseX * rotationSpeed;
        transform.rotation = Quaternion.Euler(0, playerYawRotation, 0);
    }

    /// <summary>
    /// player movement control
    /// </summary>
    /// <param name="Forward">the forward input</param>
    /// <param name="Strafe">the sideways input</param>
    private void MovePlayer(float Forward, float Strafe, bool run)
    {
        if (eating) { return; }
        float speed = run ? runSpeed : moveSpeed;
        Vector3 moveVect = new Vector3(Strafe * strafeSpeed, rigidBody.velocity.y, Forward * speed);
        moveVect = Quaternion.Euler(0, playerYawRotation, 0) * moveVect;

        // if the player is grounded use a velosity based movement else use force based
        if (IsGrounded()) rigidBody.velocity = moveVect;
        else if (IsSwimming()) rigidBody.AddForce(moveVect * swimControlFactor);
        else rigidBody.AddForce(moveVect * airControlFactor);
    }

    /// <summary>
    /// checks for the jump key and jumps
    /// </summary>
    private void CheckForJump()
    {
        if (eating) { return; }
        if (Input.GetKey(jumpKey) && (IsGrounded() || IsSwimming()))
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
        if (Input.GetKeyUp(camSwitchKey))
        {
            if (!camSwiched)
            {
                isTPP = !isTPP;
                cam.GetComponent<Camera>().cullingMask = isTPP ?
                    LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Water", "UI", "Ignore FP Camera") : 
                    LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Water", "UI");
                camTransform.parent = isTPP ? null : this.gameObject.transform;   
                camSwiched = true;
            }
        }
        else camSwiched = false;
    }

    /// <summary>
    /// checks if there is a collision whitin the distance of 0.01f below the player
    /// </summary>
    public bool IsGrounded()
    {
        Vector3 colliderBottom = new Vector3(playerCollider.bounds.center.x, playerCollider.bounds.min.y, playerCollider.bounds.center.z);
        float sphereRadius = 1.01f;
        Vector3 checkSperePoint = colliderBottom + new Vector3(0, sphereRadius, 0);
        bool grounded = Physics.CheckSphere(checkSperePoint, sphereRadius);
        
        // old raycast based method
        //bool grounded = Physics.Linecast(playerCollider.bounds.center, colliderBottom);



        Debug.DrawLine(checkSperePoint, checkSperePoint + Vector3.down * sphereRadius, grounded ? Color.red : Color.blue);

        return grounded;
    }

    public bool IsSwimming()
    {
        return (transform.position.y <= swimHeight);
    }

    public void FloatInWater()
    {
        Vector3 pos = transform.position;
        pos.y = Mathf.Max(transform.position.y, swimHeight);
        transform.position = pos;
        if (IsSwimming())
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, Mathf.Max(rigidBody.velocity.y, 0), rigidBody.velocity.z);
    }


    /// <summary>
    /// third person camera update tick
    /// </summary>
    void TPPCamUpdate()
    {
        Vector3 characterOffset = transform.position + new Vector3(0, TPPDistanceUp, 0);

        //lookDir = characterOffset - camTransform.position;
        //lookDir.y = 0;
        //lookDir.Normalize();

        TPPLookDir = transform.forward;


        Debug.DrawRay(camTransform.position, TPPLookDir, Color.green);

        TPPTargetPos = characterOffset + transform.up * TPPDistanceUp - TPPLookDir * TPPDistanceAway;
        Debug.DrawLine(transform.position, TPPTargetPos, Color.magenta);

        TPPCompensateForWalls(characterOffset, ref TPPTargetPos);
        TPPSmoothPosition(camTransform.position, TPPTargetPos);

        camTransform.LookAt(characterOffset);
    }

    /// <summary>
    /// dampens the movemantof the third person cam
    /// </summary>
    private void TPPSmoothPosition(Vector3 fromPos, Vector3 toPos)
    {
        camTransform.position = Vector3.SmoothDamp(fromPos, toPos, ref velocityCanSmooth, TPPCanSmoothDampTime);
    }

    /// <summary>
    /// makes sure the third person can does not clip into walls
    /// </summary>
    private void TPPCompensateForWalls(Vector3 fromObject, ref Vector3 toTarget)
    {
        Debug.DrawLine(fromObject, toTarget, Color.cyan);

        RaycastHit wallHit = new RaycastHit();
        if (Physics.Linecast(fromObject, toTarget, out wallHit))
        {
            Debug.DrawRay(wallHit.point, Vector3.down, Color.red);
            toTarget = new Vector3(wallHit.point.x, toTarget.y, wallHit.point.z);
        }
    }
    private void HandleAnimation(float WS, bool run)
    {
        animator.SetBool("isWalking", WS != 0);
        animator.SetBool("isRunning", run);
        eating = animator.GetCurrentAnimatorStateInfo(0).IsName("eat");
        animator.SetBool("isSwimming", IsSwimming());
    }

    public void StartEatAnimation()
    {
        animator.SetTrigger("eat");
    }

}
