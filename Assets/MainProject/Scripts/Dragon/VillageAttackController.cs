using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageAttackController : MonoBehaviour
{
    public Animator anim = null;
    private InputManager inputManager;
    private GameManager gameManager;
    private UXManager uxManager;
    public ShootProjectile fireballControl;
    public ShootProjectile firebombControl;
    enum Attack {FIREBALL, FIREBREATH, FIREBOMB};
    private Attack currentAttackType;
    public Camera mainCamera;
    public Transform cursorTarget;


    [Header ("Customization related")]
    public SkinnedMeshRenderer horns;
    public SkinnedMeshRenderer tail;
    
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
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit)) {
            cursorTarget.position = raycastHit.point;
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
                anim.SetTrigger("FireBall");
                Vector3 dir = (raycastHit.point - fireballControl.throwpoint.transform.position).normalized;
                fireballControl.direction = dir;
            }
            else if (currentAttackType == Attack.FIREBREATH)
            {
                anim.SetTrigger("Firebreath");
                // Direction setting TBD
            }
            else if (currentAttackType == Attack.FIREBOMB)
            {
                anim.SetTrigger("FireBomb");
                Vector3 dir = (raycastHit.point - firebombControl.throwpoint.transform.position).normalized;
                firebombControl.direction = dir;
            }
        }
    }
}
