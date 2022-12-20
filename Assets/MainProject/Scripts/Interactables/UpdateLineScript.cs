using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateLineScript : MonoBehaviour
{
    [SerializeField] LineRenderer line;

    public Transform _parentTransform;
    public Transform _childTransform;

    private Vector3[] positions = new Vector3[2];

    public void Start(){
        line.positionCount = 2;
    }


    private void Update()
    {
        positions[0] = _parentTransform.position;
        positions[1] = _childTransform.position; 

        line.SetPositions(positions);
    }
}
