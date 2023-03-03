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

    public GameObject FireballEvent;
    public GameObject FlameThrowerEvent;
    public GameObject EnemyManager;
    public float enemyStartDelay;
    private GameObject Trashcan;
    private GameObject EnemyContainer;

    public GameObject BarrelSpawner;
    public GameObject BarrelAI;
    public bool BarrelLive;
    public Transform barrelExplosionPoint;
    public GameObject explosionEffect;
    
    bool EndGame;

    [Header ("Customization related")]
    public SkinnedMeshRenderer horns;
    public SkinnedMeshRenderer tail;


    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        Cursor.visible = false;
        inputManager = InputManager.Instance;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uxManager = GameObject.Find("GameManager").GetComponent<UXManager>();
        Trashcan = GameObject.Find("Trashcan");
        StartCoroutine (StartEnemies());
        // Grab values from game manager for dragon appearance
        horns.SetBlendShapeWeight(0, gameManager.hornSqueeze);
        horns.SetBlendShapeWeight(1, gameManager.hornSize);
        horns.SetBlendShapeWeight(2, gameManager.hornCurve);
        tail.gameObject.SetActive(gameManager.tailSpikeEnabled);
        tail.SetBlendShapeWeight(0, gameManager.tailSqueeze);
        tail.SetBlendShapeWeight(1, gameManager.tailSize);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit)) {
            transform.position = raycastHit.point;
        }
        

        if (inputManager.DragonLeftClickThisFrame()) {
            Dragon.GetComponent<Animator>().SetTrigger("FireBall");
            Vector3 dir = (raycastHit.point - FireballEvent.GetComponent<ShootProjectile>().throwpoint.transform.position).normalized;
            FireballEvent.GetComponent<ShootProjectile>().direction = dir;


        }

        if (inputManager.DragonRightClickThisFrame()) {
           //This will be the flame thrower
        }

        if (gameManager.HealthAmount <= 0.25f && EndGame == false) {
            EndGame = true;
            foreach (Transform child in Trashcan.transform) {
                Destroy(child.gameObject);
            }
            uxManager.LoadScene(6);
        }


        if (!BarrelLive) {
            BarrelLive = true;
            StartCoroutine (SpawnBarrel());
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
        yield return new WaitForSeconds (10f);
        GameObject BarrelPig = Instantiate (BarrelAI, BarrelSpawner.transform.position, Quaternion.identity);
        
    }

    public IEnumerator BarrelNuke() {
        GameObject explosion = Instantiate (explosionEffect, barrelExplosionPoint.position, Quaternion.identity);
        EnemyContainer = GameObject.Find("EnemyContainer");
        yield return new WaitForSeconds (0.5f);
        foreach (Transform child in EnemyContainer.transform) {
            child.gameObject.GetComponent<CastleAttackAI>().Dead = true;
        }
        BarrelLive = false;
        
        
    }




}
