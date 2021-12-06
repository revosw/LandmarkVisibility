using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkyExposure : MonoBehaviour
{
    public Camera cam;
    public Vector3 mousePos;
    public float SkyExposurePercentage;

    public Text skyExpText;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        // Left click
        if (Input.GetMouseButtonDown(0))
        {

            // Shoot a ray towards world coordinate
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycast;

            // If the ray hit the ground, save the hit world coordinate
            if (Physics.Raycast(ray, out raycast, float.MaxValue, 1 << LayerMask.NameToLayer("Ground")))
            {
                mousePos = raycast.point;

                // Start calculating sky exposure
                CalculateSkyExposure();
            }

            // Update UI Label
            skyExpText.text = ("Sky visibility: " + SkyExposurePercentage);
        }
    }

    void CalculateSkyExposure()
    {
        int counter_sky = 0;
        int counter_total = 0;
        int i, j;

        // Iterate through all angles between 30° and 150° in x
        for (i = 30; i <= 150; i++)
        {
            // Iterate through all angles between 60° and -60° in y
            for (j = 60; j >= -60; j--)
            {
                counter_total++;

                var pitch = Mathf.Cos(i * Mathf.PI / 180);
                var yaw = Mathf.Sin(i * Mathf.PI / 180) * Mathf.Cos(j * Mathf.PI / 180);
                var roll = Mathf.Sin(i * Mathf.PI / 180) * Mathf.Sin(j * Mathf.PI / 180);

                Vector3 rayDirection = new Vector3(pitch, yaw, roll);
                Ray ray = new Ray(mousePos, rayDirection);
                RaycastHit hit;

                Debug.DrawRay(mousePos, rayDirection, Color.white, 5f);
                if (Physics.Raycast(ray, out hit, float.MaxValue, 1 << LayerMask.NameToLayer("Default")))
                {
                    // If an object obstructed the ray, increase counter
                    counter_sky++;

                }
            }
        }

        // Percentage is how many of the total rays were not obstructed by an object
        SkyExposurePercentage = (1 - counter_sky * 1.0f / counter_total) * 100;
    }

}
