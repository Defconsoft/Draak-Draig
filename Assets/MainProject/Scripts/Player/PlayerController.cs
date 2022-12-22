using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;


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
    public bool isTree;
    public bool stopFollowing;
    public bool nextScene;
    public int SceneToLoad;
    public Transform interactingObject;
    public GameObject exitTrigger;
    private bool inNPC;
    private Quaternion rot;
    public GameObject arms;


    private CharacterController controller;
    private GameManager gameManager;
    private UXManager uXManager;
    private Vector3 playerVelocity;
    private InputManager inputManager;
    private Transform cameraTransform;
    

    [Header ("Cameras")]
    public CinemachineVirtualCamera FirstPersonCam;
    public CinemachineVirtualCamera InteractCam;
    public Transform followObject;
    public Transform stopFollowObject;
    
    [Header ("footstep Audio")]
    [Range(0f, 1f)]
    public float audioClipVolume = 0.1f;
    //Range of random volume deviation used for footsteps;
    //Footstep audio clips will be played at different volumes for a more "natural sounding" result;
    public float relativeRandomizedVolumeRange = 0.2f;
    public AudioClip[] footStepClips;
    //Footsteps will be played every time the traveled distance reaches this value 
    public float footstepDistance = 1f;
    float currentFootstepDistance = 0f;
    public AudioSource audioSource;


    private void Start()
    {

        controller = GetComponent<CharacterController>();
        inputManager = InputManager.Instance;
        cameraTransform = Camera.main.transform;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uXManager = GameObject.Find("GameManager").GetComponent<UXManager>();


        //Grabs the  variables from the Game Manager
        playerSpeed = gameManager.playerSpeed;
        jumpHeight = gameManager.jumpHeight;
        gravityValue = gameManager.gravityValue;


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
            if (move.magnitude > 0)
            {
                currentFootstepDistance += Time.deltaTime * playerSpeed;

				//Play foot step audio clip if a certain distance has been traveled;
				if(currentFootstepDistance > footstepDistance)
				{
					//Only play footstep sound if mover is grounded 
					if(groundedPlayer)
                    {
						PlayFootstepSound();
                    }
					currentFootstepDistance = 0f;
				}
            } 
            

            // Changes the height position of the player..
            if (inputManager.PlayerJumpedThisFrame() && groundedPlayer)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            }

            if (inputManager.PlayerInteractThisFrame()) {

                if (stateInteract){
                    Interacting = true;

                    stopFollowObject.position = followObject.position;
                    stopFollowObject.rotation = followObject.rotation;
                    FirstPersonCam.m_Follow = stopFollowObject;
                    
                    

                    //Set the interact camera angle
                    interactingObject.gameObject.GetComponent<Interactable>().itemCanvas.DOFade(0,1f);
                    Vector3 lookPos;
                    if (isTree) {
                        lookPos = new Vector3 (interactingObject.position.x, interactingObject.position.y + 2f, interactingObject.position.z)  - cameraTransform.position;
                    } else {
                        lookPos = interactingObject.position - cameraTransform.position;
                    }
                    
                    rot = Quaternion.LookRotation(lookPos);
    
                    //Quaternion rot = Quaternion.Euler (0f, cameraTransform.eulerAngles.y, 0f);
                    InteractCam.ForceCameraPosition(cameraTransform.position, rot);
                    //Switch the cameras
                    FirstPersonCam.m_Priority = 0;
                    InteractCam.m_Priority = 10;
                    arms.SetActive(true);
                    StartCoroutine(StopFollow());


                    if (interactingObject.gameObject.GetComponent<Interactable>().isNPC) {
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                        inNPC = true;
                        interactingObject.gameObject.GetComponent<InteractableNPCManager>().StartConversation();
                    } else {
                        inNPC = false;
                        interactingObject.GetChild(2).GetComponent<InteractableGameManager>().StartTheGame();
                    }

                } else if (nextScene){
                    Destroy(exitTrigger);
                    uXManager.LoadScene(SceneToLoad);
                    
                }

            }

        

            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);

        } 



        if (inputManager.PlayerClickedThisFrame()) {
            if (!inNPC){
                if (interactingObject != null){
                    if (interactingObject.GetChild(2).GetComponent<InteractableGameManager>().InteractableState == 1){
                        interactingObject.GetChild(2).GetComponent<InteractableGameManager>().KillTheAimTween();
                    } else if (interactingObject.GetChild(2).GetComponent<InteractableGameManager>().InteractableState == 2) {
                        interactingObject.GetChild(2).GetComponent<InteractableGameManager>().KillThePowerTween();
                        //Endinteracting();
                    } else {
                        return;
                    }
                }
            } 
        }

    }

    public void Endinteracting(){

        Interacting = false;
        //Switch the cameras
        Quaternion rot = Quaternion.Euler (cameraTransform.eulerAngles.x, cameraTransform.eulerAngles.y, cameraTransform.eulerAngles.z);
        FirstPersonCam.ForceCameraPosition(cameraTransform.position, rot);
        FirstPersonCam.m_Priority = 10;
        InteractCam.m_Priority = 0;
        arms.SetActive(false);
        StartCoroutine(StartFollow());
        
    }


    public IEnumerator StopFollow(){
        yield return new WaitForSeconds(1f);
        stopFollowing = true;
        yield return new WaitForEndOfFrame();
    }

    public IEnumerator StartFollow(){
        FirstPersonCam.m_Follow = followObject;
        stopFollowing = false;
        yield return new WaitForSeconds (cameraTransform.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time);
    }

    void PlayFootstepSound()
		{
			int _footStepClipIndex = Random.Range(0, footStepClips.Length);
			audioSource.PlayOneShot(footStepClips[_footStepClipIndex], audioClipVolume + audioClipVolume * Random.Range(-relativeRandomizedVolumeRange, relativeRandomizedVolumeRange));
		}
}
