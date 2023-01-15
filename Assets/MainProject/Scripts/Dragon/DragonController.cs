using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class DragonController : MonoBehaviour
{

    private Rigidbody rb;
    private InputManager inputManager;
    private GameManager gameManager;
    private UXManager uXManager;

    private Vector3 dragonVelocity;
    private bool groundedDragon;
    private float currentVelocity;
    private PlayerControls controls;


    [Header ("Cameras")]
    public CinemachineVirtualCamera TopDownCam;
    public CinemachineVirtualCamera EagleEyeCam;
    private CinemachineBrain mainCamBrain;


    private bool EagleActive;
    private bool canEagle;

    [SerializeField] private float dragonSpeed = 2.0f;
    [SerializeField] private float rotateSpeed = 2.0f;
    [SerializeField] private float eagleBlendTime = 0.3f;
    [SerializeField] private float eagleAmount = 1f;

    private void Awake() {
        controls = new PlayerControls();
    }


    private void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        inputManager = InputManager.Instance;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uXManager = GameObject.Find("GameManager").GetComponent<UXManager>();
        mainCamBrain = Camera.main.GetComponent<CinemachineBrain>();

        //Grabs the  variables from the Game Manager
        //dragonSpeed = gameManager.dragonSpeed;
        //rotateSpeed = gameManager.dragonRotateSpeed;
    }



    private void Update() {
        EagleActive = controls.Dragon.RightMouse.ReadValue<float>() > 0;


        if (EagleActive && eagleAmount> 0) {
            DecreaseEagleAmount();
            canEagle = true;
        } else if (EagleActive && eagleAmount == 0) {
            canEagle = false;
        } else if (!EagleActive && eagleAmount > 0) {
            IncreaseEagleAmount();
            canEagle = false;
        } else {
            IncreaseEagleAmount();
        }

        uXManager.SetEagleEyeAmount(eagleAmount);


        if (inputManager.DragonSwoopedThisFrame()) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Camera.main.transform.position);

            if (Physics.Raycast (ray, out hit)) {
                if (hit.transform.gameObject.tag == "forestHit") {
                    Debug.Log ("GOT HIM");
                } else {
                    Debug.Log ("MISSED");
                }
            }

        }


    }

    // Update is called once per frame
    void FixedUpdate()
    {
    
        rb.velocity = transform.forward * dragonSpeed * Time.deltaTime;
    
        if (Input.GetKey(KeyCode.A))
        {
            transform.RotateAround(transform.position, -Vector3.up, rotateSpeed * Time.deltaTime);
        } 
        else if (Input.GetKey(KeyCode.D))
        {
            transform.RotateAround(transform.position, Vector3.up, rotateSpeed * Time.deltaTime);
        }     


        if (canEagle) {
            mainCamBrain.m_DefaultBlend.m_Time = 0.3f;
            TopDownCam.m_Priority = 0;
            EagleEyeCam.m_Priority = 10;
        } else {
            TopDownCam.m_Priority = 10;
            EagleEyeCam.m_Priority = 0;
        }



    }




    private void DecreaseEagleAmount(){
            eagleAmount = eagleAmount -= Time.deltaTime;
            eagleAmount = Mathf.Clamp(eagleAmount, 0f, 1f);
    }

    private void IncreaseEagleAmount(){
            eagleAmount = eagleAmount += Time.deltaTime;
            eagleAmount = Mathf.Clamp(eagleAmount, 0f, 1f);
    }


    private IEnumerator BrainReset() {
        yield return new WaitForSeconds(eagleBlendTime);
        mainCamBrain.m_DefaultBlend.m_Time = 1f;
    }


}
