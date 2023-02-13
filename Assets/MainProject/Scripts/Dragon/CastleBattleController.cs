using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleBattleController : MonoBehaviour
{


    [SerializeField] private Camera mainCamera;



    public Texture2D cursorTexture;
    public CursorMode cursorMode= CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        Cursor.SetCursor (cursorTexture, hotSpot, cursorMode);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit)) {
            transform.position = raycastHit.point;
        }
        
    }
}
