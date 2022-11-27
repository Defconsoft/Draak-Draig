using UnityEngine;
using DG.Tweening;

public class Interactable : MonoBehaviour
{

    public enum InteractType { rock, tree, fish, NPC};

    [Header("Object Attributes")]
    public float radius = 3.0f;
    private SphereCollider areaTrigger;
    public InteractType interactType;
    public bool isNPC;

    public GameObject Objects;
 

    [Header("UX stuff")]
    public CanvasGroup itemCanvas;



    private void Awake() {
        areaTrigger = gameObject.AddComponent<SphereCollider>();
        areaTrigger.radius = radius;
        areaTrigger.isTrigger = true;
    }


    private void Start() {
        
        //Setup the item type
        if (interactType == InteractType.rock){
            GameObject rockClone = Instantiate(Objects, transform);
        } 

        if (interactType == InteractType.tree){

        } 

        if (interactType == InteractType.fish){

        } 

        if (interactType == InteractType.NPC){
            isNPC = true;
        }              

    }


    //handles the actual interactivity
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            itemCanvas.DOFade (1f,1f);
            other.GetComponent<PlayerController>().stateInteract = true;
            other.GetComponent<PlayerController>().interactingObject = this.transform;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            itemCanvas.DOFade (0f,1f);
            other.GetComponent<PlayerController>().stateInteract = false;
        }        
    }





    //visualises the trigger in Editor
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere (transform.position, radius);
    }


}
