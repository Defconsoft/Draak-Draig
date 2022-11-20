using UnityEngine;

public class Interactable : MonoBehaviour
{

    public enum InteractType { rock, tree, fish};

    [Header("Object Attributes")]
    public float radius = 3.0f;
    private SphereCollider areaTrigger;
    public InteractType interactType;

    public GameObject Rocks;
    public GameObject Trees;
    public GameObject FishSpots;


    [Header("UX stuff")]
    public Canvas itemCanvas;



    private void Awake() {
        areaTrigger = gameObject.AddComponent<SphereCollider>();
        areaTrigger.radius = radius;
        areaTrigger.isTrigger = true;
    }


    private void Start() {
        
        //Setup the item type
        if (interactType == InteractType.rock){
            GameObject rockClone = Instantiate(Rocks, transform);
        } 

        if (interactType == InteractType.tree){

        } 

        if (interactType == InteractType.fish){

        }             

    }











    //handles the actual interactivity
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            itemCanvas.enabled = true;
            other.GetComponent<PlayerController>().stateInteract = true;
            other.GetComponent<PlayerController>().interactingObject = this.transform;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            itemCanvas.enabled = false;
            other.GetComponent<PlayerController>().stateInteract = false;
        }        
    }





    //visualises the trigger in Editor
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere (transform.position, radius);
    }


}
