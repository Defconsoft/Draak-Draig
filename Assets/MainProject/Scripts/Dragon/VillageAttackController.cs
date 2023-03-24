using System.Collections;
using System.Collections.Generic;
using PathCreation.Examples;
using UnityEngine;

public class VillageAttackController : MonoBehaviour
{
    public Animator anim = null;
    private InputManager inputManager;
    private GameManager gameManager;
    private UXManager uxManager;
    public Camera mainCamera;

    [Header("Attack logic")]
    public ShootProjectile fireballControl;
    public ShootProjectile firebombControl;
    public FirebreathControl fireBreath;
    enum Attack {FIREBALL, FIREBREATH, FIREBOMB};
    private Attack currentAttackType;
    public float rechargeSpeedFireBall = 3f;
    public float rechargeSpeedFireBreath = 2f;
    public float rechargeSpeedFireBomb = 1f;
    public Texture2D cursorTexture;
    private Vector2 hotSpot;
    public Transform aimControl;
    private float fireBallCharge = 1f;
    private float fireBreathCharge = 1f;
    private float fireBombCharge = 1f;
    private bool canFireBall = true;
    private bool canFireBreath = true;
    private bool canFireBomb = true;

    [Header ("Destruction score handling")]
    public float destructionGoal = 100f;
    private float destructionAmount = 0f;


    [Header ("Customization related")]
    public SkinnedMeshRenderer horns;
    public SkinnedMeshRenderer tail;

    public float flapFrequency = 0.01f;
    private float timeSinceFlap = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        if (anim == null)
        {
            anim = GetComponentInChildren<Animator>();
        }

        inputManager = InputManager.Instance;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uxManager = GameObject.Find("GameManager").GetComponent<UXManager>();
        // mainCamera = Camera.main;

        // Grab values from game manager for dragon appearance
        horns.SetBlendShapeWeight(1, gameManager.hornSqueeze);
        horns.SetBlendShapeWeight(0, gameManager.hornSize);
        horns.SetBlendShapeWeight(2, gameManager.hornCurve);
        tail.gameObject.SetActive(gameManager.tailSpikeEnabled);
        tail.SetBlendShapeWeight(0, gameManager.tailSqueeze);
        tail.SetBlendShapeWeight(1, gameManager.tailSize);

        currentAttackType = Attack.FIREBALL;

        // Subscribe to target hit event to keep track of score
        EventManager.TargetHit += IncreaseDestructionScore;

        // Setting the aiming cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        hotSpot = new Vector2 (cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        // Flap wings occasionally
        timeSinceFlap += 1f;
        if (timeSinceFlap >= 1f/flapFrequency)
        {
            anim.SetTrigger("FlapWings");
            timeSinceFlap = 0f;
        } 
        
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit)) {
            aimControl.position = raycastHit.point;
        }
        
        if (inputManager.DragonSwitchToFireball())
        {
            currentAttackType = Attack.FIREBALL;
            uxManager.SetAttackType((int) Attack.FIREBALL);
        }
        else if (inputManager.DragonSwitchToFirebreath())
        {
            currentAttackType = Attack.FIREBREATH;
            uxManager.SetAttackType((int) Attack.FIREBREATH);
        }
        else if (inputManager.DragonSwitchToFirebomb())
        {
            currentAttackType = Attack.FIREBOMB;
            uxManager.SetAttackType((int) Attack.FIREBOMB);
        }
        
        if (inputManager.DragonLeftClickThisFrame()) {

            if (currentAttackType == Attack.FIREBALL)
            {
                if (canFireBall)
                {
                    anim.SetTrigger("FireBall");
                    // Vector3 dir = (raycastHit.point - fireballControl.throwpoint.transform.position).normalized;
                    // fireballControl.direction = dir;
                    fireballControl.target = raycastHit.point;
                    fireBallCharge = 0f;
                    canFireBall = false;
                    uxManager.SetAttackCharge((int) currentAttackType, 1f - fireBallCharge);
                    StartCoroutine(ReChargeFireBall());
                }
                
            }
            else if (currentAttackType == Attack.FIREBREATH)
            {
                if (canFireBreath)
                {
                    anim.SetTrigger("Firebreath");
                    fireBreath.target = raycastHit.point;
                    fireBreathCharge = 0f;
                    canFireBreath = false;
                    uxManager.SetAttackCharge((int) currentAttackType, 1f - fireBreathCharge);
                    StartCoroutine(ReChargeFireBreath());
                }
            }
            else if (currentAttackType == Attack.FIREBOMB)
            {
                if (canFireBomb)
                {
                    anim.SetTrigger("FireBomb");
                    // Vector3 dir = (raycastHit.point - firebombControl.throwpoint.transform.position).normalized;
                    // firebombControl.direction = dir;
                    firebombControl.target = raycastHit.point;
                    fireBombCharge = 0f;
                    canFireBomb = false;
                    uxManager.SetAttackCharge((int) currentAttackType, 1f - fireBombCharge);
                    StartCoroutine(ReChargeFireBomb());
                }
                
            }
        }
    }

    IEnumerator ReChargeFireBall()
    {
        while (fireBallCharge < 1f)
        {
            fireBallCharge += rechargeSpeedFireBall * 0.01f;
            uxManager.SetAttackCharge((int) Attack.FIREBALL, 1f - fireBallCharge);
            yield return new WaitForSeconds(0.1f);
        }
        Mathf.Clamp(fireBallCharge, 0f, 1f);
        uxManager.SetAttackCharge((int) Attack.FIREBALL, 0f);
        canFireBall = true;
    }

    IEnumerator ReChargeFireBreath()
    {
        while (fireBreathCharge < 1f)
        {
            fireBreathCharge += rechargeSpeedFireBreath * 0.01f;
            uxManager.SetAttackCharge((int) Attack.FIREBREATH, 1f - fireBreathCharge);
            yield return new WaitForSeconds(0.1f);
        }
        Mathf.Clamp(fireBreathCharge, 0f, 1f);
        uxManager.SetAttackCharge((int) Attack.FIREBREATH, 1f - fireBreathCharge);
        canFireBreath = true;
    }

    IEnumerator ReChargeFireBomb()
    {
        while (fireBombCharge < 1f)
        {
            fireBombCharge += rechargeSpeedFireBomb * 0.01f;
            uxManager.SetAttackCharge((int) Attack.FIREBOMB, 1f - fireBombCharge);
            yield return new WaitForSeconds(0.1f);
        }
        Mathf.Clamp(fireBombCharge, 0f, 1f);
        uxManager.SetAttackCharge((int) Attack.FIREBOMB, 0f);
        canFireBomb = true;
    }

    private void IncreaseDestructionScore(float targetValue)
    {
        destructionAmount += targetValue;
        if (destructionAmount >= destructionGoal)
        {
            Debug.Log("Destruction goal reached");
            destructionAmount = destructionGoal;
            // Trigger win message
        }
        uxManager.SetDestructionAmount(destructionAmount/destructionGoal);
    }

}
