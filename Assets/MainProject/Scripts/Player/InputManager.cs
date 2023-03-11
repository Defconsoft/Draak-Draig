using UnityEngine;

public class InputManager : MonoBehaviour
{

    private static InputManager _instance;

    public static InputManager Instance {
        get {
            return _instance;
        }
    }

    private PlayerControls playerControls;


    private void Awake() {
        //Setup singleton
        if (_instance != null && _instance != this){
            Destroy (this.gameObject);
        } else {
            _instance = this;
        }

        playerControls = new PlayerControls();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }

    //Player controls

    public Vector2 GetPlayerMovement(){
        return playerControls.Player.Movement.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta(){
        return playerControls.Player.Look.ReadValue<Vector2>();
    }

    public bool PlayerJumpedThisFrame() {
        return playerControls.Player.Jump.triggered;
    }

    public bool PlayerInteractThisFrame() {
        return playerControls.Player.Interact.triggered;
    }

    public bool PlayerClickedThisFrame() {
        return playerControls.Player.MouseFire.triggered;
    }


    //Dragon Controls

    public Vector2 GetDragonMovement(){
        return playerControls.Dragon.Movement.ReadValue<Vector2>();
    }

    public Vector2 GetDragonMouseDelta(){
        return playerControls.Dragon.MouseLook.ReadValue<Vector2>();
    }

    public bool DragonLeftClickThisFrame() {
        return playerControls.Dragon.LeftMouse.triggered;
    }

    public bool DragonRightClickThisFrame() {
        return playerControls.Dragon.RightMouse.triggered;
    }

    public bool DragonSwitchToFireball() {
        return playerControls.Dragon.FireBall.triggered;
    }

    public bool DragonSwitchToFirebreath() {
        return playerControls.Dragon.FireBreath.triggered;
    }

    public bool DragonSwitchToFirebomb() {
        return playerControls.Dragon.FireBomb.triggered;
    }


}
