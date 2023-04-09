using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlyingController : MonoBehaviour
{

    public FlyingCameraController mainCamera;
    public Transform dragonMesh;
    private InputManager inputManager;
    private GameManager gameManager;
    private UXManager uXManager;
    private GameObject Dragon;
    private PlayerControls controls;
    private Camera cam;
    //Movement variables
    public float dragonThrust = 10000f;
    private float tempThrust;
    public float pitchSpeed = 30f;
    public float rollSpeed = 45f;
    public float yawSpeed = 25f;
    public float autoTurnAngle = 60f;
    public float startingSpeed = 100f;
    public RectTransform crosshairs;
    public Animator anim;
    public Transform target;
    public FirebreathControl fireBreath;
    bool firing;
    public bool canMove = true;

    [SerializeField] private float fireBreathAmount;
    private bool BreathActive;
    bool canFirebreath;


    private Rigidbody rb;

    private float thrust, pitch, roll, yaw; //Globals
    private const float aeroDynamicEffect = 0.1f;
    private bool enableMouseControls;

    internal bool showCrosshairs;
    internal Vector3 crosshairPosition;

    private void Awake() {
        //GRAB THE RB
        controls = new PlayerControls();
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        
    }


    private void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();
    }



    private void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        thrust = startingSpeed;
        Dragon = GameObject.Find("dragonImproved");
        inputManager = InputManager.Instance;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uXManager = GameObject.Find("GameManager").GetComponent<UXManager>();
        //rb.AddForce(transform.forward * 500f, ForceMode.VelocityChange);
        tempThrust = dragonThrust;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (canMove){
            BreathActive = controls.Dragon.RightMouse.ReadValue<float>() > 0;

                if (BreathActive && fireBreathAmount <= 0.24f) {
                    canFirebreath = false;
                    IncreaseFirebreathAmount();
                } else if (BreathActive && fireBreathAmount > 0.24f) {
                    if (!firing){
                        canFirebreath = true;
                    }
                IncreaseFirebreathAmount();
                } else {
                    IncreaseFirebreathAmount();
                }

                if (fireBreathAmount <= 0.24f) {
                    uXManager.powerText.SetActive (true);
                } else {
                    uXManager.powerText.SetActive (false);
                }

                uXManager.SetFireBreathAmount(fireBreathAmount);


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

            // Firebreath
            if (inputManager.DragonRightClickThisFrame() && canFirebreath)
            {
                
                anim.SetTrigger("Firebreath");
                fireBreath.target = target.position;
                canFirebreath = false;
                DecreaseFirebreathAmount();
                StartCoroutine (breathDelay());
            }
        }

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
        if (Mathf.Abs(newAngle) > 45f)
        {
            anim.SetTrigger("FlapWings");
        }

        pitch = -Mathf.Clamp(localTarget.y, -1f, 1f);
        roll = Mathf.Clamp (newAngle, -1f, 1f);
        yaw = Mathf.Clamp (localTarget.x, -1f, 1f);

    }

    float FindAngle( float v) {
        if (v > 180f) v -= 360f;
        return v;
    }


    private void FixedUpdate() {

        if (canMove){
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
    }

    void LateUpdate() {
        crosshairPosition = cam.WorldToScreenPoint (transform.position + (transform.forward * 500f));
    }

    void OnCollisionEnter(Collision collision)
    {
        // Ignore colliding into birds as that logic is taken care of in the bird and firebreath scripts
        if (collision.gameObject.tag != "bird")
        {
            anim.SetTrigger("HitWorld");
        }
    }

    private void DecreaseFirebreathAmount(){
            fireBreathAmount = fireBreathAmount - 0.25f;
            fireBreathAmount = Mathf.Clamp(fireBreathAmount, 0f, 1f);
            
;    }

    private void IncreaseFirebreathAmount(){
            fireBreathAmount = fireBreathAmount += Time.deltaTime * 0.02f;
            fireBreathAmount = Mathf.Clamp(fireBreathAmount, 0f, 1f);
    }

    IEnumerator breathDelay() {
        firing = true;
        yield return new WaitForSeconds (2f);
        canFirebreath = true;
        firing = false;
    }


}
