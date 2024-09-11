using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private GameObject cameraObject;
    private Transform camTransform;

    [SerializeField] private float distanceAway;
    [SerializeField] private float distanceUp;

    private Vector3 velocityCanSmooth = Vector3.zero;
    [SerializeField] private float canSmoothDampTime = .1f;

    private Vector3 lookDir;
    private Vector3 targetPos;

    /// <summary>
    /// setup on spawn
    /// </summary>
    void Start()
    {
        Debug.Assert(cameraObject != null, "Third person camera in not attached");
        camTransform = cameraObject.transform;
        
        // takes the camera out of the player prefab so its position is not confined
        camTransform.parent = null;
    }

    void LateUpdate()
    {
        Vector3 characterOffset = transform.position + new Vector3(0, distanceUp, 0);

        lookDir = characterOffset - camTransform.position;
        lookDir.y = 0;
        lookDir.Normalize();

        Debug.DrawRay(camTransform.position, lookDir, Color.green);

        targetPos = characterOffset + transform.up * distanceUp - lookDir * distanceAway;
        Debug.DrawLine(transform.position, targetPos, Color.magenta);

        CompensateForWalls(characterOffset, ref targetPos);
        SmoothPosition(camTransform.position, targetPos);

        camTransform.LookAt(characterOffset);
    }

    private void SmoothPosition(Vector3 fromPos, Vector3 toPos)
    {
        camTransform.position = Vector3.SmoothDamp(fromPos, toPos, ref velocityCanSmooth, canSmoothDampTime);
    }

    private void CompensateForWalls(Vector3 fromObject, ref Vector3 toTarget)
    {
        Debug.DrawLine(fromObject, toTarget, Color.cyan);

        RaycastHit wallHit = new RaycastHit();
        if (Physics.Linecast(fromObject, toTarget, out wallHit))
        {
            Debug.DrawRay(wallHit.point, Vector3.down, Color.red);
            toTarget = new Vector3(wallHit.point.x, toTarget.y, wallHit.point.z);
        }
    }
}