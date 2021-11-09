using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SahdowDetect : MonoBehaviour
{
    //variables' names
    [SerializeField] private Vector3 position;
    [SerializeField] private int shadow_counter;
    [SerializeField] private LayerMask _layers;

    [SerializeField] private List<Light> Lights;

    //[SerializeField] public float LightInterval;

    public GameObject cube;
    public GameObject plane;

    private int[,] lightMatrix;

    void Start()
    {
        //(Vetcor3 V3, Vector2 size) = GetPoints(plane, 1f);
       // lightMatrix = new int[size.x, size.z];

    }

    void Awake(){
        Lights = new List<Light>(FindObjectsOfType<Light>()); 
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(180,transform.localEulerAngles.y, transform.localEulerAngles.z),1.0f);
    }

     void Update()
     {

        if(Input.GetMouseButtonDown(0)) 
        {
            foreach (Vector3 V3 in GetPoints(plane, 1f))
            {
                shadow_counter = 0;
                Light light = Lights[0];
                if (light == null) { return; }

                int i;
                for (i = 0; i <= 180; ++i)
                {
                    transform.localEulerAngles = new Vector3(i, transform.localEulerAngles.y, transform.localEulerAngles.z);
                    if (IsInDirectionalLightShadow(light, V3))
                    {
                        ++shadow_counter;
                    }
                }

                print("Shadow counter " + shadow_counter);
            }
        }
    }      

    bool IsInDirectionalLightShadow(Light light, Vector3 target_position)
        {
            RaycastHit hit;
            Ray ray = new Ray(target_position, -light.transform.forward);
            Debug.DrawRay(ray.origin, ray.direction, Color.red);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1<<LayerMask.NameToLayer("Default")))
            {
                return false;
            }
            return true;
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