using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LightIntensityBar : MonoBehaviour
{
    private Light sun;
    private Text percentageText;
    private Vector3 surfaceNormal;
    float intensity;
    
    private void Start()
    {
        sun = FindObjectOfType<Light>();
        percentageText = gameObject.transform.parent.GetComponentInChildren<Text>();
        surfaceNormal = GetSurfaceNormal();
    }

    void Update()
    {
        intensity = getLightIntensity();
        percentageText.text = $"{(int)(intensity * 100)}%";
        transform.localScale = new Vector3(1, intensity * 50, 1);
    }

    Vector3 GetSurfaceNormal()
    {
        // the ray comes from just above the point and point down to intersect with the surface.
        Vector3 worldCoordinate = transform.position + Vector3.up;
        Ray directionalRay = new Ray(worldCoordinate, -Vector3.up);
        RaycastHit raycast;

        if (Physics.Raycast(directionalRay, out raycast, Mathf.Infinity, 1 << LayerMask.NameToLayer("Ground")))
        {
            //normalVector is the normal vector of the specific point at the surface.
            return raycast.normal;
        }

        return Vector3.zero;
    }

    float getLightIntensity()
    {
        float angleDifference = Vector3.Angle(surfaceNormal, sun.transform.forward);

        //if it is night, the light intensity should be 0.
        if (angleDifference < 90) return 0;

        // Else, get a value between 0 and 1, where 1 means
        // the sun is directly facing the surface normal
        return (angleDifference - 90) / 90; 
    }
}
