using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SphereController : MonoBehaviour
{

    private Vector3 originPosition;
    private Vector3 cameraPosition;

    private Collider hitBuilding;

    private Vector3 rayDirection;

    public GameObject landmark;

    public bool activate;

    public float Visibiliyt_Index;
    private float totalRays = 0;
    private float raysHit = 0;
    private int rotationPoints = 360;
    private float landmarkHeight;

    public Text visIndexDisplay;

    // Start is called before the first frame update
    void Start()
    {
        cameraPosition = GameObject.Find("Sphere").transform.position;
        originPosition = cameraPosition;
        rayDirection = Vector3.forward;

        Mesh mesh = landmark.GetComponent<MeshFilter>().mesh;
        Bounds bounds = mesh.bounds;
        landmarkHeight = bounds.size[1]; // Hight of landmark
    }


    public void activateLandmark()
    {
        RaycastHit hit;

        Visibiliyt_Index = 0;
        for (int r = 0; r < landmarkHeight; r = r + 10) // Check every 10 in hight of the landmark
        {
            for (int j = 0; j < rotationPoints; j++) // Rotate around the landmark
            {
                for (float s = 0; s < 360; s += 1.0F) // Rotate around at each point
                {
                    if (Physics.Raycast(transform.position, transform.TransformDirection(rayDirection), out hit, Mathf.Infinity))
                    {
                        //Debug.DrawRay(transform.position, transform.TransformDirection(rayDirection) * hit.distance, Color.yellow);

                        hitBuilding = hit.collider;
                        //hitBuilding.GetComponent<Renderer>().material.SetColor("_Color", Color.red);

                        totalRays++; // Count rays
                        raysHit++; // Count rays that hits
                    }
                    else
                    {
                        //Debug.DrawRay(transform.position, transform.TransformDirection(rayDirection) * 1000, Color.white);
                        totalRays++; // Count rays
                    }
                    Vector3 rotate = new Vector3(0, s, 0);
                    transform.rotation = Quaternion.Euler(rotate);
                }
                
                transform.RotateAround(landmark.transform.position, Vector3.up, 1);
            }
            transform.position = new Vector3(0, r, 0);
            Debug.Log(transform.position);
        }
        Visibiliyt_Index = Mathf.Round(raysHit / totalRays * 100); // Calculate visibility index
        visIndexDisplay.text = ("Visibility index: " + Visibiliyt_Index); // Return index to canvas
    }
}
