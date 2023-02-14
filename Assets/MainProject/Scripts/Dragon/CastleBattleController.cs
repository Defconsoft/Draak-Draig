using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleBattleController : MonoBehaviour
{


    private Camera mainCamera;
    [SerializeField] private GameObject Dragon;
    private InputManager inputManager;


    public GameObject FireballEvent;
    public GameObject FlameThrowerEvent;


    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        Cursor.visible = false;
        inputManager = InputManager.Instance;



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

    }

    private void LateUpdate() {
        transform.LookAt(mainCamera.transform);
        transform.Rotate(0, 180, 0);
    }
}
