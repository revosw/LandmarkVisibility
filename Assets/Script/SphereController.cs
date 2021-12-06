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

    public float visibilityIndex;
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
        landmarkHeight = bounds.size[1] * landmark.transform.localScale.y; // Hight of landmark
    }


    public void activateLandmark()
    {
        StartCoroutine(CalculateVisibility());
    }

    IEnumerator CalculateVisibility()
    {
        RaycastHit hit;
        transform.position = originPosition;
        visibilityIndex = 0;
        for (int altitude = 1; altitude < landmarkHeight; altitude += 5) // Check every 10 in hight of the landmark
        {
            for (int circumference = 0; circumference < rotationPoints; circumference++) // Rotate around the landmark
            {
                for (int angle = 0; angle < 360; angle++) // Rotate around at each point
                {
                    if (Physics.Raycast(transform.position, transform.TransformDirection(rayDirection), out hit, Mathf.Infinity))
                    {
                        // We might shoot the ray on the grass - the terrain doesn't have a renderer component
                        // so we just skip this 
                        if (hit.collider.gameObject.CompareTag("Building"))
                        {
                            hitBuilding = hit.collider;
                            hitBuilding.GetComponent<Renderer>().material.SetColor("_Color", Color.red);

                            raysHit++; // Count rays that hits
                        }
                        Debug.DrawRay(transform.position, transform.TransformDirection(rayDirection) * hit.distance, Color.yellow);
                    }
                    else
                    {
                        Debug.DrawRay(transform.position, transform.TransformDirection(rayDirection) * 1000, Color.white);
                    }
                    totalRays++; // Count rays
                    transform.Rotate(Vector3.up);
                }

                transform.RotateAround(landmark.transform.position, Vector3.up, 1);
                //yield return null;
                yield return null;
            }
            transform.position = new Vector3(transform.position.x, altitude, transform.position.z);
            //Debug.Log(transform.position);
        }
        visibilityIndex = Mathf.Round(raysHit / totalRays * 100); // Calculate visibility index
        visIndexDisplay.text = ("Visibility index: " + visibilityIndex); // Return index to canvas
    }
}
