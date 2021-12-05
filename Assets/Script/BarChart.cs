using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class BarChart : MonoBehaviour
{
    AnimateBars animateBars;
    //private List<Light> Lights;
    //private Light light;
    private Camera cam;
    public GameObject barCube;
    public float Intensity;

    void Awake()
    {

        //Lights = new List<Light>(FindObjectsOfType<Light>()); 
        //light = Lights[0];
        cam = GetComponent<Camera>();
        //barCube= GameObject.FindWithTag("BarCube");
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickPosition = Vector3.zero;
            if (getMousePosition(out clickPosition))
            {
                Instantiate(barCube, clickPosition, Quaternion.identity);
            }
        }

        //float LightIntensity = getLightIntensity(light, clickPosition);
        //    //barCube.transform.position=clickPosition;
        //Intensity=LightIntensity;

    }

    bool getMousePosition(out Vector3 clickPosition)
    {
        Debug.Log($"{Input.mousePosition.x} {Input.mousePosition.y}");
        clickPosition = Vector3.zero;
        if (clickedInsideButtonGroup()) return false;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            //Debug.Log(hit.transform.gameObject.layer);
            clickPosition = hit.point;
            return true;
        }
        return false;
    }

    bool clickedInsideButtonGroup()
    {
        // Left button group
        if (Input.mousePosition.y < 133 && Input.mousePosition.x < 165)
        {
            return true;
        }
        // Right button group
        else if (Input.mousePosition.y < 72 && Input.mousePosition.x > Screen.width - 165)
        {
            return true;
        }
        return false;
    }

    //float getLightIntensity(Light light, Vector3 target_position){

    //    // the ray comes from just above the point and point down to intersect with the surface.
    //    Vector3 lightOrigin = target_position + Vector3.up;
    //    Ray directionalRay = new Ray(lightOrigin, -Vector3.up);
    //    RaycastHit raycast;
    //    float LightIntensity = 0;

    //    if(Physics.Raycast(directionalRay, out raycast,Mathf.Infinity, 1<<LayerMask.NameToLayer("Ground"))){

    //        //normalVector is the normal vector of the specific point at the surface.
    //        Vector3 normalVector= raycast.normal;  
    //        float angle = Vector3.Angle(-normalVector,light.transform.forward);

    //        //if it is night, the light intensity should be 0.
    //        if(angle>90){LightIntensity=0;}
    //        else{ LightIntensity = (90-angle)/90; }

    //    }
    //    return LightIntensity;
    //} 
}
