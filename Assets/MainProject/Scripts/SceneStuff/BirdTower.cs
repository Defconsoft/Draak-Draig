using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdTower : MonoBehaviour
{
    public float radius;

    public int numBirds;
    public GameObject[] birds;
    public GameObject releasePoint;
    public int totalBirds;
    public GameObject burnEffect;



    private void Start() {
        numBirds = GameObject.Find("GameManager").GetComponent<UXManager>().BirdTowerAmount;
        for (int i = 0; i < numBirds; i++)
        {
            GameObject cloneBird = Instantiate (birds[Random.Range (0, birds.Length)], releasePoint.transform.position, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
            cloneBird.GetComponent<BirdsFlying>().homeTarget = releasePoint.transform;
            cloneBird.GetComponent<BirdsFlying>().flyingTarget = releasePoint.transform;
            cloneBird.GetComponent<BirdsFlying>().radiusMinMax.y = radius;
            cloneBird.GetComponent<BirdsFlying>().yMinMax.x = transform.position.y + 10f;
            cloneBird.GetComponent<BirdsFlying>().yMinMax.y = transform.position.y + 60f;
            cloneBird.GetComponent<BirdsFlying>().originScript = this;
            totalBirds++;
        }
    }


    public void SpawnBird() {
        if (totalBirds < numBirds) {
            GameObject cloneBird = Instantiate (birds[Random.Range (0, birds.Length)], releasePoint.transform.position, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
            cloneBird.GetComponent<BirdsFlying>().homeTarget = releasePoint.transform;
            cloneBird.GetComponent<BirdsFlying>().flyingTarget = releasePoint.transform;
            cloneBird.GetComponent<BirdsFlying>().radiusMinMax.y = radius;
            cloneBird.GetComponent<BirdsFlying>().yMinMax.x = transform.position.y + 10f;
            cloneBird.GetComponent<BirdsFlying>().yMinMax.y = transform.position.y + 60f;
            cloneBird.GetComponent<BirdsFlying>().originScript = this;
            totalBirds++;
        }
    }    


    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void Burn()
    {
        burnEffect.SetActive(true);
    }
}
