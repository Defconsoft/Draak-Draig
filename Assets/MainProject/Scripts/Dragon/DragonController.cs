using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class DragonController : MonoBehaviour
{

    private Rigidbody rb;
    private InputManager inputManager;
    private GameManager gameManager;
    private UXManager uXManager;

    private Vector3 dragonVelocity;
    private bool groundedDragon;
    private float currentVelocity;


    [SerializeField] private float dragonSpeed = 2.0f;
    [SerializeField] private float rotateSpeed = 2.0f;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        inputManager = InputManager.Instance;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uXManager = GameObject.Find("GameManager").GetComponent<UXManager>();

        //Grabs the  variables from the Game Manager
        //dragonSpeed = gameManager.dragonSpeed;
        //rotateSpeed = gameManager.dragonRotateSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    
        rb.velocity = transform.forward * dragonSpeed * Time.deltaTime;
    
        if (Input.GetKey(KeyCode.A))
        {
            transform.RotateAround(transform.position, -Vector3.up, rotateSpeed * Time.deltaTime);
        } 
        else if (Input.GetKey(KeyCode.D))
        {
            transform.RotateAround(transform.position, Vector3.up, rotateSpeed * Time.deltaTime);
        }     


    }


}
