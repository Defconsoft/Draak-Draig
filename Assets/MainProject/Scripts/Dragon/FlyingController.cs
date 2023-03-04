using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingController : MonoBehaviour
{

    public FlyingCameraController mainCamera;
    public Transform dragonMesh;
    private Camera cam;
    //Movement variables
    public float dragonThrust = 10000f;
    public float pitchSpeed = 30f;
    public float rollSpeed = 45f;
    public float yawSpeed = 25f;
    public float autoTurnAngle = 60f;
    public float startingSpeed = 100f;
    public RectTransform crosshairs;


    private Rigidbody rb;

    private float thrust, pitch, roll, yaw; //Globals
    private const float aeroDynamicEffect = 0.1f;
    private bool enableMouseControls;

    internal bool showCrosshairs;
    internal Vector3 crosshairPosition;

    private void Awake() {
        //GRAB THE RB
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;

    }

    private void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        thrust = startingSpeed;
        //rb.AddForce(transform.forward * 500f, ForceMode.VelocityChange);
    
    }

    // Update is called once per frame
    void Update()
    {
        //Clear out old values
        pitch = 0f;
        roll = 0f;
        yaw = 0f;

        //Update control surfaces
        if (Input.GetKey(KeyCode.Q)) yaw = -1f;
        if (Input.GetKey(KeyCode.E)) yaw = 1f;

        if (Input.GetKey(KeyCode.A)) roll = 1f;
        if (Input.GetKey(KeyCode.D)) roll = -1f;

        if (Input.GetKey(KeyCode.W)) pitch = -1f;
        if (Input.GetKey(KeyCode.S)) pitch = 1f;

        UpdateThrottle();
        UpdateCamera();
        CheckMouseControls();

        crosshairs.position = crosshairPosition;


    }

    void UpdateThrottle(){

        if (Input.GetKey(KeyCode.Backspace)) thrust = 100f;

        if (Input.GetKey(KeyCode.KeypadPlus)) thrust += 10f;
        if (Input.GetKey(KeyCode.KeypadMinus)) thrust -= 10f;

        thrust = Mathf.Clamp(thrust, 0f, 100f);

    }

    void UpdateCamera(){

        mainCamera.updatePosition (Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw ("Mouse Y"));
        
    }

    void CheckMouseControls(){

        var localTarget = transform.InverseTransformDirection(cam.transform.forward).normalized * 5f;

        var targetRollAngle = Mathf.Lerp (0, autoTurnAngle, Mathf.Abs (localTarget.x));
        if (localTarget.x > 0f) targetRollAngle *= -1f;

        var rollAngle = FindAngle(dragonMesh.transform.localEulerAngles.z);
        var newAngle = targetRollAngle - rollAngle;

        pitch = -Mathf.Clamp(localTarget.y, -1f, 1f);
        roll = Mathf.Clamp (newAngle, -1f, 1f);
        yaw = Mathf.Clamp (localTarget.x, -1f, 1f);

    }

    float FindAngle( float v) {
        if (v > 180f) v -= 360f;
        return v;
    }


    private void FixedUpdate() {


        transform.RotateAround (transform.position, transform.up, yaw * Time.fixedDeltaTime * yawSpeed);  //yaw
        dragonMesh.transform.RotateAround (dragonMesh.transform.position, dragonMesh.transform.forward, roll * Time.fixedDeltaTime * rollSpeed);  //roll
        transform.RotateAround (transform.position, transform.right, pitch * Time.fixedDeltaTime * pitchSpeed);  //pitch

        //Auto Level
        var rotateSpeed = Mathf.Clamp (transform.right.y, -1f, 1) * -1f;
        if (Mathf.Abs (pitch) > 0.1f) {
            transform.RotateAround (transform.position, transform.forward, rotateSpeed);  
        }



        var localVelocity = transform.InverseTransformDirection (rb.velocity);
        var localSpeed = Mathf.Max (0, localVelocity.z);

        var aeroFactor = Vector3.Dot(transform.forward, rb.velocity.normalized);
        aeroFactor *= aeroFactor;
        rb.velocity = Vector3.Lerp (rb.velocity, transform.forward * localSpeed, aeroFactor * localSpeed * aeroDynamicEffect * Time.fixedDeltaTime);

        rb.AddForce ((thrust * dragonThrust) * transform.forward);

    }

    void LateUpdate() {
        crosshairPosition = cam.WorldToScreenPoint (transform.position + (transform.forward * 500f));
    }
}
