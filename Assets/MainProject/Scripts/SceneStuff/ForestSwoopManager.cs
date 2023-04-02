using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForestSwoopManager : MonoBehaviour
{


    public int totalNumberEnemies;
    public int currentNumberEnemies;
    private GameManager gameManager;
    public GameObject enemyPrefab;
    private Vector3 spawnPosition;
    private Vector3 randomPosition;
    private bool initialSpawn;

    public Vector3 centre;
    public Vector3 size;


    // Start is called before the first frame update
    void Start()
    {

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();


        if (gameManager.totalWood <= 100) {
            totalNumberEnemies = 25;
        } else if (gameManager.totalWood >= 101 && gameManager.totalWood <= 250) {
            totalNumberEnemies = 60;
        } else if (gameManager.totalWood >= 251 ) {
            totalNumberEnemies = 100;
        }

        gameManager.totalWood = 0;

        for (int i = 0; i < totalNumberEnemies; i++)
        {
            SpawnEnemies();
        }

        initialSpawn = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (initialSpawn) {
            if (currentNumberEnemies < totalNumberEnemies) {
                SpawnEnemies();
            }
        }

    }
   
   
    private void SpawnEnemies(){

        randomPosition = (transform.localPosition + centre) + new Vector3 (Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2), Random.Range(-size.z / 2, size.z / 2));

        RaycastHit hit;
        Ray ray = new Ray (randomPosition, Vector3.down);
        Physics.Raycast (ray, out hit);

        if (hit.collider.tag =="forestGround") {
            spawnPosition = hit.point;
        } else {
            return;
        }

        GameObject clone = Instantiate (enemyPrefab, spawnPosition, Quaternion.identity);

        currentNumberEnemies++;

    }


    void OnDrawGizmosSelected() {
        Gizmos.color = new Color (1,0,0,0.5f);
        Gizmos.DrawCube(transform.localPosition + centre, size);   
    }


}
