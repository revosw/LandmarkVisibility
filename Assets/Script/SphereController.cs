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

    public Text visIndexDisplay;

    // Start is called before the first frame update
    void Start()
    {
        cameraPosition = GameObject.Find("Sphere").transform.position;
        originPosition = cameraPosition;
        rayDirection = Vector3.forward; 
    }


    public void activateLandmark()
    {
        RaycastHit hit;

        Visibiliyt_Index = 0;
        for (int r = 0; r < 1; r++)
        {
            for (int j = 0; j < rotationPoints; j++)
            {
                for (float s = 0; s < 360; s += 1.0F)
                {
                    if (Physics.Raycast(transform.position, transform.TransformDirection(rayDirection), out hit, Mathf.Infinity))
                    {
                        Debug.DrawRay(transform.position, transform.TransformDirection(rayDirection) * hit.distance, Color.yellow);

                        hitBuilding = hit.collider;
                        //hitBuilding.GetComponent<Renderer>().material.SetColor("_Color", Color.red);

                        totalRays++;
                        raysHit++;
                    }
                    else
                    {
                        Debug.DrawRay(transform.position, transform.TransformDirection(rayDirection) * 1000, Color.white);
                        totalRays++;
                    }
                    Vector3 test = new Vector3(0, s, 0);
                    transform.rotation = Quaternion.Euler(test);
                }
                Visibiliyt_Index = Mathf.Round(raysHit / totalRays * 100);
                transform.RotateAround(landmark.transform.position, Vector3.up, 1);
            }
        }

        visIndexDisplay.text = ("Visibility index: " + Visibiliyt_Index);
    }
}
