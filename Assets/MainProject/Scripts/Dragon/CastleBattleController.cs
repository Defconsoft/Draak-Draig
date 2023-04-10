using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CastleBattleController : MonoBehaviour
{


    private Camera mainCamera;
    [SerializeField] private GameObject Dragon;
    private InputManager inputManager;
    private GameManager gameManager;
    private UXManager uxManager;
    private PlayerControls controls;

    public GameObject FireballEvent;
    public FirebreathControl fireBreath;
    bool firing;
    public GameObject EnemyManager;
    public float enemyStartDelay;
    private GameObject Trashcan;
    private GameObject EnemyContainer;

    public GameObject BarrelSpawner;
    public GameObject BarrelAI;
    public bool BarrelLive;
    public Transform barrelExplosionPoint;
    public GameObject explosionEffect;

    [SerializeField] private float fireBreathAmount;
    private bool BreathActive;
    bool canFirebreath = true;
    float barrelDelay = 10f;

    bool EndGame;

    [Header ("Customization related")]
    public SkinnedMeshRenderer horns;
    public SkinnedMeshRenderer tail;


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
        mainCamera = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        inputManager = InputManager.Instance;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uxManager = GameObject.Find("GameManager").GetComponent<UXManager>();
        Trashcan = GameObject.Find("Trashcan");
        StartCoroutine (StartEnemies());
        // Grab values from game manager for dragon appearance
        horns.SetBlendShapeWeight(1, gameManager.hornSqueeze);
        horns.SetBlendShapeWeight(0, gameManager.hornSize);
        horns.SetBlendShapeWeight(2, gameManager.hornCurve);
        tail.gameObject.SetActive(gameManager.tailSpikeEnabled);
        tail.SetBlendShapeWeight(0, gameManager.tailSqueeze);
        tail.SetBlendShapeWeight(1, gameManager.tailSize);
        barrelDelay = uxManager.BarrelDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (uxManager.isPaused == false){
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
                uxManager.powerText.SetActive (true);
            } else {
                uxManager.powerText.SetActive (false);
            }

            uxManager.SetFireBreathAmount(fireBreathAmount);





            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit)) {
                transform.position = raycastHit.point;
            }
            

            if (inputManager.DragonLeftClickThisFrame()) {
                Dragon.GetComponent<Animator>().SetTrigger("FireBall");
                FireballEvent.GetComponent<ShootProjectile>().target = raycastHit.point;
            }

            if (inputManager.DragonRightClickThisFrame() && canFirebreath) {
            canFirebreath = false;
            StartCoroutine (breathDelay());
            DecreaseFirebreathAmount();
            //This will be the flame thrower
            Dragon.GetComponent<Animator>().SetTrigger("Firebreath");
            // fireBreathPoint.LookAt(raycastHit.point);
            fireBreath.target = raycastHit.point;
            
            
            }

            if (gameManager.HealthAmount <= 0.25f && EndGame == false) {
                EndGame = true;
                uxManager.BarrelDelay = 10f;
                foreach (Transform child in Trashcan.transform) {
                    Destroy(child.gameObject);
                }
                uxManager.LoadScene(8);
            }


            if (!BarrelLive) {
                BarrelLive = true;
                StartCoroutine (SpawnBarrel());
            }
        }

    }

    private void LateUpdate() {
        transform.LookAt(mainCamera.transform);
        transform.Rotate(0, 180, 0);
    }


    IEnumerator StartEnemies() {
        yield return new WaitForSeconds (enemyStartDelay);
        EnemyManager.SetActive (true);
    }


    IEnumerator SpawnBarrel() {
        yield return new WaitForSeconds (barrelDelay);
        GameObject BarrelPig = Instantiate (BarrelAI, BarrelSpawner.transform.position, Quaternion.identity);
        
    }

    public IEnumerator BarrelNuke() {
        GameObject explosion = Instantiate (explosionEffect, barrelExplosionPoint.position, Quaternion.identity);
        EnemyContainer = GameObject.Find("EnemyContainer");
        yield return new WaitForSeconds (0.5f);
        foreach (Transform child in EnemyContainer.transform) {
            child.gameObject.GetComponent<CastleAttackAI>().Dead = true;
        }

        foreach (Transform child in Trashcan.transform) {
            Destroy (child.gameObject);
        }
        BarrelLive = false;
        
        
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
