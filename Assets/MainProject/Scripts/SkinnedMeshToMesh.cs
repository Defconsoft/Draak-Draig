using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SkinnedMeshToMesh : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMesh;
    public VisualEffect vfxGraph;
    public float refreshRate = 0.02f;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateVFXGraph());
        
    }

    IEnumerator UpdateVFXGraph(){
        while(gameObject.activeSelf)
        {
            Mesh m = new Mesh();
            skinnedMesh.BakeMesh(m);
            Vector3[] vertices = m.vertices;
            
            // Random fix necessary to make the new mesh work properly in VFXgraph
            Mesh m2 = new Mesh();
            m2.vertices = vertices;
            vfxGraph.SetMesh("Mesh", m2); 

            yield return new WaitForSeconds(refreshRate);
        }
    }

}
