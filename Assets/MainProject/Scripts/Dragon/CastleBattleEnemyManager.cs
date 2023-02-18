using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleBattleEnemyManager : MonoBehaviour
{


    public enum SpawnArea { left, right, back};
    public SpawnArea spawnArea;
    public GameObject EnemySpawnpoint;

    public GameObject enemyObject;
    private GameObject EnemyContainer;
    
    public float maxSpawnTimeWait;

    public bool hasEnemy;


    private void Start() {
        EnemyContainer = GameObject.Find("EnemyContainer");
    }


    private void Update() {
        
        if (!hasEnemy) {
            
            StartCoroutine(SpawnEnemy());
        }

        
    }


    IEnumerator SpawnEnemy() {
        hasEnemy = true;
        float randomTime = Random.Range (0, maxSpawnTimeWait);

        yield return new WaitForSeconds(randomTime);

        GameObject enemy = Instantiate (enemyObject, EnemySpawnpoint.transform.position, Quaternion.identity);
        enemy.GetComponent<CastleAttackAI>().destination = transform;
        enemy.GetComponent<CastleAttackAI>().enemyManager = this;
        enemy.transform.parent = EnemyContainer.transform;
        if (maxSpawnTimeWait > 0) {maxSpawnTimeWait--;} //reduce the timer


    }







}
