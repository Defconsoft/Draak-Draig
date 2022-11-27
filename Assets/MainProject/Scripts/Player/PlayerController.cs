using UnityEngine;
using Cinemachine;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    [Header ("PlayerAttributes")]
    [SerializeField] private bool groundedPlayer;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;

    [Header ("StateInteracting")]
    public bool Interacting;
    public bool stateInteract;
    public bool nextScene;
    public int SceneToLoad;
    public Transform interactingObject;


    private CharacterController controller;
    private GameManager gameManager;
    private UXManager uXManager;
    private Vector3 playerVelocity;
    private InputManager inputManager;
    private Transform cameraTransform;

    [Header ("Cameras")]
    public CinemachineVirtualCamera FirstPersonCam;
    public CinemachineVirtualCamera InteractCam;
    


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        inputManager = InputManager.Instance;
        cameraTransform = Camera.main.transform;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uXManager = GameObject.Find("GameManager").GetComponent<UXManager>();
    }

    void Update()
    {

        //Move Player if not interacting
        if (!Interacting){
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }

            Vector2 movement = inputManager.GetPlayerMovement();
            Vector3 move = new Vector3 (movement.x, 0f, movement.y);
            move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
            move.y = 0f;
            controller.Move(move * Time.deltaTime * playerSpeed);

            // Changes the height position of the player..
            if (inputManager.PlayerJumpedThisFrame() && groundedPlayer)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            }

            if (inputManager.PlayerInteractThisFrame()) {

                if (stateInteract){
                    Interacting = true;
                    //Set the interact camera angle
                    interactingObject.gameObject.GetComponent<Interactable>().itemCanvas.enabled = false;
                    Vector3 lookPos = interactingObject.position - cameraTransform.position;
                    Quaternion rot = Quaternion.LookRotation(lookPos);                
                    //Quaternion rot = Quaternion.Euler (0f, cameraTransform.eulerAngles.y, 0f);
                    InteractCam.ForceCameraPosition(cameraTransform.position, rot);
                    //Switch the cameras
                    FirstPersonCam.m_Priority = 0;
                    InteractCam.m_Priority = 10;
                    interactingObject.GetChild(1).GetComponent<InteractableGameManager>().StartTheGame();
                } else if (nextScene){
                    uXManager.LoadScene(SceneToLoad);
                }

            }

        

            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);

        } 



        if (inputManager.PlayerClickedThisFrame()) {
            if (interactingObject.GetChild(1).GetComponent<InteractableGameManager>().InteractableState == 1){
                interactingObject.GetChild(1).GetComponent<InteractableGameManager>().KillTheAimTween();
            } else if (interactingObject.GetChild(1).GetComponent<InteractableGameManager>().InteractableState == 2) {
                interactingObject.GetChild(1).GetComponent<InteractableGameManager>().KillThePowerTween();
                Endinteracting();
            } else {
                return;
            }
        }

    }

    public void Endinteracting(){

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Interacting = false;
        //Switch the cameras
        Quaternion rot = Quaternion.Euler (cameraTransform.eulerAngles.x, cameraTransform.eulerAngles.y, cameraTransform.eulerAngles.z);
        FirstPersonCam.ForceCameraPosition(cameraTransform.position, rot);
        InteractCam.m_Priority = 0;
        FirstPersonCam.m_Priority = 10;

        
    }





}
