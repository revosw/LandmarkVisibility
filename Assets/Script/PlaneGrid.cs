using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneGrid : MonoBehaviour
{

    public GameObject cube;
    public GameObject plane;

    void Start()
    {
    }

        


    
    void FixedUpdate()
    {

        foreach (Vector3 V3 in GetPoints(plane, 1f))
        {
            cube.transform.localPosition = V3;
            print(V3);
        }
    }

    public static IEnumerable<Vector3> GetPoints(GameObject obj, float res)
    {
        
        Vector3[] verts = obj.GetComponent<MeshFilter>().sharedMesh.vertices;
        float x1 = float.MaxValue;
        float x2 = 0;
        float z1 = float.MaxValue;
        float z2 = 0;
        foreach (Vector3 vert in verts)
        {
            if (vert.x < x1)
            {
                x1 = vert.x;
            }
            if (vert.x > x2)
            {
                x2 = vert.x;
            }
            if (vert.z < z1)
            {
                z1 = vert.z;
            }
            if (vert.z > z2)
            {
                z2 = vert.z;
            }
        }
        Vector2 size = new Vector2(Mathf.Abs(x1 - x2), Mathf.Abs(z1 - z2));
        for (float x = -(size.x / 2); x <= size.x / 2; x += res)
        {
            for (float z = -(size.y / 2); z <= size.y / 2; z += res)
            {
                Vector3 V3 = obj.transform.TransformPoint(new Vector3(x, 1, z));
                    yield return V3;
            }
        }
    }
}
