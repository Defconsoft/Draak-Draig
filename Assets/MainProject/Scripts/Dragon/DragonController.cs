using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;

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


    [Header ("Forest Swoop Stuff")]
    private bool EagleActive;
    private bool canEagle;
    private bool killing;
    private GameObject tempPig;
    private GameObject tempDragonModel;
    private GameObject tempPigModel;
    private GameObject tempEndSpot;
    private bool hpFlip;
    private bool loadingVillageAttack;
    private bool loadingOpeningScene;
    private bool stormDamageActive;
    public Volume volume;
    Vignette vignette;
    PaniniProjection paniniProjection;
    bool takingDamage;
    public float stormDelay;
    public Canvas StormWarning;
    public ShootProjectile fireballControl;
    public Color missColor;
    public TextMeshProUGUI missText;

    [SerializeField] private float dragonSpeed = 2.0f;
    [SerializeField] private float rotateSpeed = 2.0f;
    [SerializeField] private float eagleBlendTime = 0.3f;
    [SerializeField] private float eagleAmount = 1f;
    [SerializeField] private CanvasGroup aimReticle;
    private Image aimReticleImage;

    [Header ("Animation stuff")]
    public Animator anim;
    public float flapFrequency = 0.01f;
    private float timeSinceFlap = 0f;
    private float tiltMin = -1f;
    private float tiltMax = 1f;
    private float tiltIncrement = 0.1f;
    private float tilt = 0f;
    private Animator tempAnim;

    [Header ("Customization related")]
    public SkinnedMeshRenderer horns;
    public SkinnedMeshRenderer tail;

    [Header ("Death")]
    public CanvasGroup DeathUI;


    private void Awake() {
        controls = new PlayerControls();
        anim.ResetTrigger("FlapWings");
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
        volume.profile.TryGet<Vignette>(out vignette);
        volume.profile.TryGet<PaniniProjection>(out paniniProjection);
        //Grabs the  variables from the Game Manager
        dragonSpeed = gameManager.dragonSpeed;
        rotateSpeed = gameManager.dragonRotateSpeed;
        horns.SetBlendShapeWeight(1, gameManager.hornSqueeze);
        horns.SetBlendShapeWeight(0, gameManager.hornSize);
        horns.SetBlendShapeWeight(2, gameManager.hornCurve);
        tail.gameObject.SetActive(gameManager.tailSpikeEnabled);
        tail.SetBlendShapeWeight(0, gameManager.tailSqueeze);
        tail.SetBlendShapeWeight(1, gameManager.tailSize);

        // Set anim correctly
        anim.SetFloat("Tilt", tilt);

        aimReticleImage = aimReticle.gameObject.GetComponentInChildren<Image>();
    }



    private void Update() {

        bool eagleWasActiveBefore = EagleActive;
        if (eagleWasActiveBefore && !(controls.Dragon.RightMouse.ReadValue<float>() > 0))
            {
                // Turn off time slow
                Time.timeScale = 1f;
            }

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


            if (uXManager.isPaused == false){
                if (inputManager.DragonLeftClickThisFrame()) {
                    tilt = 0f;
                    anim.SetFloat("Tilt", tilt);
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Camera.main.transform.position);

                    if (Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward * 100f, out hit)) {
                        if (hit.collider.gameObject.tag == "forestHit" || hit.collider.gameObject.tag == "forestOuter") {
                            hit.collider.gameObject.transform.parent.gameObject.GetComponent<ForestSwoopAI>().caught = true;
                            hit.collider.gameObject.transform.parent.gameObject.GetComponent<Rigidbody>().isKinematic = true;


                            if (hit.collider.gameObject.tag == "forestHit"){
                                //grab the stuff I need
                                tempPig = hit.collider.gameObject.transform.parent.gameObject;
                                tempPig.GetComponent<ForestSwoopAI>().PlayPigSound();
                                tempPig.GetComponent<ForestSwoopAI>().SetAnim(1);
                                tempKillCam = tempPig.GetComponent<ForestSwoopAI>().KillCam;
                                tempDragonModel = tempPig.GetComponent<ForestSwoopAI>().dragonModel;
                                tempPigModel = tempPig.GetComponent<ForestSwoopAI>().pigModel;
                                tempEndSpot = tempPig.GetComponent<ForestSwoopAI>().endSpot;
                                tempAnim = tempDragonModel.GetComponent<Animator>();

                                StartCoroutine (KillAnimate());
                            } else if (hit.collider.gameObject.tag == "forestOuter") {
                                //dragonSpeed = 0f;
                                tempPig = hit.collider.gameObject.transform.parent.gameObject;
                                tempPig.GetComponent<ForestSwoopAI>().PlayPigSound();
                                StartCoroutine (KillNonAnimate());
                            }

                            
                        }
                        else 
                        {
                            anim.SetTrigger("Swoop");
                            StartCoroutine(MissfireFeedback());
                        }
                    }
                }
                else
                {
                    anim.ResetTrigger("Swoop");
                }
            }
        }


        if (stormDamageActive) {
            vignette.intensity.value = Mathf.PingPong (Time.time * 2, 0.5f);
            //paniniProjection.distance.value = Mathf.PingPong (Time.time * 2, 1);
            StormWarning.enabled = true;            
            if (!takingDamage){
                StartCoroutine(TakeStormDamage());
            }
        } else {
            vignette.intensity.value = Mathf.MoveTowards (vignette.intensity.value , 0f, 0.2f * Time.deltaTime);
            //paniniProjection.distance.value = Mathf.MoveTowards (paniniProjection.distance.value , 0.1f, 0.2f * Time.deltaTime);
            StormWarning.enabled = false;
        }

        if (gameManager.HealthAmount >= 1f && gameManager.EnergyAmount >= 1f) {
            Debug.Log ("FIRE");
            if (!loadingVillageAttack){
            loadingVillageAttack = true;
            uXManager.DragonGroupFade(0f);
            uXManager.LoadScene(9);
            }
        }

        if (gameManager.HealthAmount <= 0) {

            DeathUI.interactable = true;
            DeathUI.blocksRaycasts = true;
            DeathUI.DOFade (1, 0.5f);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }

    }

    public void DeathBtn() {
        DeathUI.DOFade (0, 0.5f);
        if (!loadingOpeningScene){
            loadingOpeningScene = true;    
            uXManager.DragonGroupFade(0f);
            uXManager.LoadScene(3);
            
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
                if (tilt > tiltMin){
                    tilt -= tiltIncrement;
                }
                transform.RotateAround(transform.position, -Vector3.up, rotateSpeed * Time.deltaTime);
            } 
            else if (Input.GetKey(KeyCode.D))
            {
                if (tilt < tiltMax){
                    tilt += tiltIncrement;
                }
                transform.RotateAround(transform.position, Vector3.up, rotateSpeed * Time.deltaTime);
            }
            else
            {
                // No input means we can start tilting back to base
                if (tilt > 0.01f)
                {
                    tilt -= tiltIncrement;
                }
                else if (tilt < -0.01f)
                {
                    tilt += tiltIncrement;
                }
                else
                {
                    tilt = 0f;
                }
            }
            anim.SetFloat("Tilt", tilt);

            // Flap wings occasionally
            timeSinceFlap += 1f;
            if (timeSinceFlap >= 1f/flapFrequency)
            {
                anim.SetTrigger("FlapWings");
                timeSinceFlap = 0f;
            }  


            if (canEagle) {
                mainCamBrain.m_DefaultBlend.m_Time = 0.3f;
                TopDownCam.m_Priority = 0;
                EagleEyeCam.m_Priority = 10;
                Time.timeScale = 0.65f;
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
        tempAnim.SetTrigger("Swoop");
        tempDragonModel.transform.DOMove (tempPig.transform.position, 1f);
        Vector3 tempPos = new Vector3 (0, 0, tempPig.transform.position.z + 1f);
        yield return new WaitForSeconds (1.5f);
        tempPigModel.transform.parent = tempDragonModel.transform;
        tempDragonModel.transform.DOMove (tempEndSpot.transform.position, 2f);
        yield return new WaitForSeconds (1.5f);
        Destroy(tempPig);
        mainCamBrain.m_DefaultBlend.m_Time = 1;
        tempKillCam.m_Priority = 0;
        ManageAttributes();
        uXManager.DragonGroupFade(1f);
        aimReticle.alpha = 1f;
        killing = false;
    }

    IEnumerator KillNonAnimate() {
        killing = true;
        tempPig.GetComponent<ForestSwoopAI>().SetAnim(1);
        NavMeshAgent temp = tempPig.GetComponent<NavMeshAgent>();
        temp.enabled = false;
        ///////////////////////////
        //FIRE THE FIREBALL HERE
        ////////////////////////////
        anim.SetTrigger("FireBall");
        fireballControl.target = tempPig.transform.position;
        yield return new WaitForSeconds(1.6f);

        Destroy(tempPig);
        ManageAttributes();
        killing = false;
        dragonSpeed = 400f;
    }

    void ManageAttributes(){
        if (gameManager.HealthAmount != 1f && gameManager.EnergyAmount != 1f){
            if (hpFlip){
                gameManager.PlusHealth(gameManager.ForestSwoopReplenish);
                hpFlip = false;
            } else {
                gameManager.PlusEnergy(gameManager.ForestSwoopReplenish);
                hpFlip = true;
            }
        } else if (gameManager.HealthAmount == 1f && gameManager.EnergyAmount != 1f) {
                gameManager.PlusEnergy(gameManager.ForestSwoopReplenish);
        } else if (gameManager.HealthAmount != 1f && gameManager.EnergyAmount == 1f) {
                gameManager.PlusHealth(gameManager.ForestSwoopReplenish);
        }

    }


    private void DecreaseEagleAmount(){
            eagleAmount = eagleAmount -= Time.deltaTime * 0.5f;
            eagleAmount = Mathf.Clamp(eagleAmount, 0f, 1f);
    }

    private void IncreaseEagleAmount(){
            eagleAmount = eagleAmount += Time.deltaTime * 0.5f;
            eagleAmount = Mathf.Clamp(eagleAmount, 0f, 1f);
    }


    private IEnumerator BrainReset() {
        yield return new WaitForSeconds(eagleBlendTime);
        mainCamBrain.m_DefaultBlend.m_Time = 1f;
    }


    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "forestStorm") {
            stormDamageActive = true;
        }
    }

        private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "forestStorm") {
            stormDamageActive = false;
        }
    }


    IEnumerator TakeStormDamage(){
        takingDamage = true;
        gameManager.MinusHealth(gameManager.ForestSwoopReplenish);
        yield return new WaitForSeconds (stormDelay);
        takingDamage = false;
    }

    IEnumerator MissfireFeedback()
    {
        missText.gameObject.SetActive(true);
        aimReticleImage.color = missColor;
        aimReticleImage.transform.DOPunchScale(new Vector3(-0.3f, -0.3f, -0.3f), 0.3f, 1, 0.3f);
        missText.transform.DOScale(0.7f, 0.5f);
        yield return new WaitForSeconds(0.25f);
        aimReticleImage.color = Color.white;
        yield return new WaitForSeconds(0.25f);
        missText.gameObject.SetActive(false);
    }

}
