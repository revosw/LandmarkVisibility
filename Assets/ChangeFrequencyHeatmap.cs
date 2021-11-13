using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFrequencyHeatmap : MonoBehaviour
{

    [Range(1,600)] [SerializeField] int resolution = 3;
    [SerializeField] GameObject plane;
    [SerializeField] GameObject directionalLight;
    Vector3[,] rayOrigins;
    // Start is called before the first frame update
    void Start()
    {
        rayOrigins = new Vector3[resolution,resolution];
        for (int i = 0; i < resolution; i++)
        {
            for(int j = 0; j < resolution; j++)
            {
                Vector3 offset = new Vector3(i * plane.transform.localScale.x * 10/resolution, 0, j * plane.transform.localScale.z * 10/resolution);
                rayOrigins[i, j] = gameObject.transform.position + offset;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If the sun is facing UP, that means it's night, so we don't have
        // to calculate anything
        //if (Vector3.Dot(directionalLight.transform.forward, Vector3.up) > 0) return;
        //for (int i = 0; i < resolution; i++)
        //{
        //    for (int j = 0; j < resolution; j++)
        //    {
        //        Debug.DrawRay(rayOrigins[i, j], -directionalLight.transform.forward * 100, Color.white);
        //        //GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = rayOrigins[i, j];
        //    }
        //}
    }
}
