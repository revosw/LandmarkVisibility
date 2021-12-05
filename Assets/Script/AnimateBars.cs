using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AnimateBars : MonoBehaviour
{
    private Light sun;
    private Text percentageText;
    float intensity;
    // Start is called before the first frame update
    private void Start()
    {
        sun = FindObjectOfType<Light>();
        percentageText = gameObject.transform.parent.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        intensity = getLightIntensity();
        percentageText.text = $"{(int)(intensity*100)}%";
        transform.localScale = new Vector3(1, intensity * 50, 1);
        //print(transform.localScale.y);
    }

    float getLightIntensity()
    {
        // the ray comes from just above the point and point down to intersect with the surface.
        Vector3 lightOrigin = transform.position + Vector3.up;
        Ray directionalRay = new Ray(lightOrigin, -Vector3.up);
        RaycastHit raycast;
        float LightIntensity = 0;

        if (Physics.Raycast(directionalRay, out raycast, Mathf.Infinity, 1 << LayerMask.NameToLayer("Ground")))
        {

            //normalVector is the normal vector of the specific point at the surface.
            Vector3 normalVector = raycast.normal;
            float angle = Vector3.Angle(normalVector, sun.transform.forward);

            Debug.DrawRay(directionalRay.origin, -sun.transform.forward * 30);
            //print(sun.transform.forward);

            //if it is night, the light intensity should be 0.
            if (angle < 90) { LightIntensity = 0; }
            else { LightIntensity = (angle - 90) / 90; }

        }
        return LightIntensity;
    }
}
