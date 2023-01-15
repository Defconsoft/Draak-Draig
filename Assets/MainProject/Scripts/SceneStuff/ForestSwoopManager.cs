using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForestSwoopManager : MonoBehaviour
{


    public int totalNumberEnemies;
    public int currentNumberEnemies;
    public GameObject enemyPrefab;
    private Vector3 spawnPosition;
    private bool initialSpawn;

    public Vector3 centre;
    public Vector3 size;


    // Start is called before the first frame update
    void Start()
    {
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

        spawnPosition = (transform.localPosition + centre) + new Vector3 (Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2), Random.Range(-size.z / 2, size.z / 2));

        RaycastHit hit;
        Ray ray = new Ray (spawnPosition, Vector3.down);
        Physics.Raycast (ray, out hit);

        if (hit.collider.tag =="forestGround") {
            spawnPosition = hit.point;
        }

        GameObject clone = Instantiate (enemyPrefab, spawnPosition, Quaternion.identity);

        currentNumberEnemies++;

    }


    void OnDrawGizmosSelected() {
        Gizmos.color = new Color (1,0,0,0.5f);
        Gizmos.DrawCube(transform.localPosition + centre, size);   
    }


}
