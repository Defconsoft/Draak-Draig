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
    public bool Interacting;
    public bool stateInteract;

    private CharacterController controller;
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
    }

    void Update()
    {

        Debug.Log (cameraTransform.localRotation.eulerAngles.y);
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

            if (inputManager.PlayerInteractThisFrame() && Interacting == false) {
                Interacting = true;
                //Set the interact camera angle
                Quaternion rot = Quaternion.Euler (0f, cameraTransform.eulerAngles.y, 0f);
                InteractCam.ForceCameraPosition(cameraTransform.position, rot);
                //Switch the cameras
                FirstPersonCam.m_Priority = 0;
                InteractCam.m_Priority = 10;

            }

        

            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);

        } else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (inputManager.PlayerInteractThisFrame() && Interacting == true) {
                Interacting = false;
                //Switch the cameras
                InteractCam.m_Priority = 0;
                FirstPersonCam.m_Priority = 10;

            } 
        }

    }

}
