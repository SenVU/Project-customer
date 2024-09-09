using UnityEngine;

public class PlayerMovament : MonoBehaviour
{
    Rigidbody rigidBody;
    [SerializeField] GameObject cam;

    [SerializeField] float moveSpeed;
    [SerializeField] float strafeSpeed;
    [SerializeField] float rotationSpeed;

    [SerializeField] float maxCamRotation;
    [SerializeField] float minCamRotation;
    [SerializeField] float camRotationSpeed;

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
        float WS = Input.GetAxis("Vertical");
        float AD = Input.GetAxis("Horizontal");
        
        RotatePlayer(mouseX);
        RotateCamera(mouseY);
    }

    private void RotateCamera(float mouseY)
    {
        Transform camTF = cam.GetComponent<Transform>();
        
        camRotation -= mouseY*camRotationSpeed;
        camRotation = Mathf.Clamp(camRotation, minCamRotation, maxCamRotation);
        camTF.rotation = Quaternion.Euler(camRotation, 0, 0);
    }

    private void RotatePlayer(float mouseX)
    {
        float rotation = transform.rotation.eulerAngles.y;
        rotation += mouseX * rotationSpeed;
        transform.rotation = Quaternion.Euler(0, rotation, 0);
    }

    private void MovePlayer(float WS, float AD)
    {

    }
}
