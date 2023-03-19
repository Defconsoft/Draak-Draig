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
    public Transform treeCameraPoint;

    public GameObject RockPrefab;
    public GameObject TreePrefab;
    public GameObject FishPrefab;
 

    [Header("UX stuff")]
    public GameObject CanvasHack;
    public CanvasGroup itemCanvas;



    private void Awake() {
        areaTrigger = gameObject.AddComponent<SphereCollider>();
        areaTrigger.radius = radius;
        areaTrigger.isTrigger = true;
        CanvasHack.SetActive (true);
    }


    private void Start() {
        
        //Setup the item type
        if (interactType == InteractType.rock){
            GameObject rockClone = Instantiate(RockPrefab, transform);
        } 

        if (interactType == InteractType.tree){
            GameObject treeClone = Instantiate(TreePrefab, transform);
        } 

        if (interactType == InteractType.fish){
            GameObject fishClone = Instantiate(FishPrefab, transform);
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
            if (interactType == InteractType.tree) {
                other.GetComponent<PlayerController>().isTree = true;
            } else {
                other.GetComponent<PlayerController>().isTree = false;
            }
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
