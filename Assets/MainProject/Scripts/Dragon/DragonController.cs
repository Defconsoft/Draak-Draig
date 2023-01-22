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
    private CinemachineVirtualCamera tempKillCam;
    private CinemachineBrain mainCamBrain;


    private bool EagleActive;
    private bool canEagle;
    private bool killing;
    private GameObject tempPig;
    private GameObject tempDragonModel;
    private GameObject tempPigModel;
    private GameObject tempEndSpot;

    [SerializeField] private float dragonSpeed = 2.0f;
    [SerializeField] private float rotateSpeed = 2.0f;
    [SerializeField] private float eagleBlendTime = 0.3f;
    [SerializeField] private float eagleAmount = 1f;
    [SerializeField] private CanvasGroup aimReticle;

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

        if (!killing){
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

                if (Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward * 100f, out hit)) {
                    Debug.DrawRay (Camera.main.transform.position, Camera.main.transform.forward * 100f, Color.red);
                    if (hit.collider.gameObject.tag == "forestHit") {
                        hit.collider.gameObject.transform.parent.gameObject.GetComponent<ForestSwoopAI>().caught = true;
                        hit.collider.gameObject.transform.parent.gameObject.GetComponent<Rigidbody>().isKinematic = true;

                        //grab the stuff I need
                        tempPig = hit.collider.gameObject.transform.parent.gameObject;
                        tempPig.GetComponent<ForestSwoopAI>().SetAnim(1);
                        tempKillCam = tempPig.GetComponent<ForestSwoopAI>().KillCam;
                        tempDragonModel = tempPig.GetComponent<ForestSwoopAI>().dragonModel;
                        tempPigModel = tempPig.GetComponent<ForestSwoopAI>().pigModel;
                        tempEndSpot = tempPig.GetComponent<ForestSwoopAI>().endSpot;

                        StartCoroutine (KillAnimate());
                    } else {
                        Debug.Log ("MISSED");
                    }
                }

            }
        }


    }

    // Update is called once per frame
    void FixedUpdate()
    {
    
        rb.velocity = Vector3.zero;

        if (!killing){
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

    }


    IEnumerator KillAnimate() {
        killing = true;
        uXManager.DragonGroupFade(0f);
        aimReticle.alpha = 0f;
        mainCamBrain.m_DefaultBlend.m_Time = 0;
        tempKillCam.m_Priority = 40;
        yield return new WaitForSeconds (1f);
        tempDragonModel.SetActive(true);
        tempDragonModel.transform.DOMove (tempPig.transform.position, 1f);
        Vector3 tempPos = new Vector3 (0, 0, tempPig.transform.position.z + 1f);
        yield return new WaitForSeconds (1f);
        tempPigModel.transform.parent = tempDragonModel.transform;
        tempDragonModel.transform.DOMove (tempEndSpot.transform.position, 2f);
        yield return new WaitForSeconds (1.5f);
        Destroy(tempPig);
        mainCamBrain.m_DefaultBlend.m_Time = 1;
        tempKillCam.m_Priority = 0;
        uXManager.DragonGroupFade(1f);
        aimReticle.alpha = 1f;
        killing = false;

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
