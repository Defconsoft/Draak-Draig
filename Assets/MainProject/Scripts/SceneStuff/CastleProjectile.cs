using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleProjectile : MonoBehaviour
{

    private GameObject dragon;
    private Rigidbody arrowRB;
    public float speed;
    Vector3 direction;
    public float damage;
    private GameManager gameManager;
    public CastleAttackAI origEnemy;
    bool fired;



    private void Awake() {
        arrowRB = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();   
    }


    void Start()
    {
        dragon = GameObject.Find("shootPoint");
        direction = dragon.transform.position - transform.position;
        StartCoroutine (FireArrow());
        
    }

    IEnumerator FireArrow() {
        yield return new WaitForSeconds (1f);
        arrowRB.velocity = direction * speed;
    }

    private void Update() {
        if (!fired) {
            if (origEnemy.deadOnce) {
                Destroy(this.gameObject);
            }
        }
    }



    private void OnTriggerEnter(Collider other) {
        
        if (other.tag == "Dragon") {
            gameManager.MinusHealth (damage);
            //WE NEED TO INSTANTITE A HIT EXPLOSION HERE.
            Destroy(gameObject);
        }


    }
}
