using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractureCleanUp : MonoBehaviour
{
    // Start is called before the first frame update
    public void CleanUp()
    {
        Destroy(gameObject, 1f);
    }
}
