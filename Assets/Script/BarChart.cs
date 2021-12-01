using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BarChart : MonoBehaviour
{

    private List<Light> Lights;
    private Light light;
    private Vector3 clickPosition;
    private Camera cam;
    public GameObject barCube;
    public float Intensity;


    void Awake(){

        Lights = new List<Light>(FindObjectsOfType<Light>()); 
        light = Lights[0];
        cam = GetComponent<Camera>();
        
        clickPosition=new Vector3(0,0,0);
        //barCube= GameObject.FindWithTag("BarCube");

    }


     void Update()
     { 
         
        if(Input.GetMouseButtonDown(0)){ 

            clickPosition = getMousePosition();
            
        }

        float LightIntensity = getLightIntensity(light, clickPosition);
            //barCube.transform.position=clickPosition;
        Intensity=LightIntensity;

     }


    Vector3 getMousePosition(){
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycast;
            if(Physics.Raycast(ray, out raycast,Mathf.Infinity, 1<<LayerMask.NameToLayer("Ground"))){
                return raycast.point;
            }
            return new Vector3(0,0,0);
    }  

    float getLightIntensity(Light light, Vector3 target_position){

        // the ray comes from just above the point and point down to intersect with the surface.
        Vector3 lightOrigin = target_position + Vector3.up;
        Ray directionalRay = new Ray(lightOrigin, -Vector3.up);
        RaycastHit raycast;
        float LightIntensity = 0;
      
        if(Physics.Raycast(directionalRay, out raycast,Mathf.Infinity, 1<<LayerMask.NameToLayer("Ground"))){
            
            //normalVector is the normal vector of the specific point at the surface.
            Vector3 normalVector= raycast.normal;  
            float angle = Vector3.Angle(-normalVector,light.transform.forward);

            //if it is night, the light intensity should be 0.
            if(angle>90){LightIntensity=0;}
            else{ LightIntensity = (90-angle)/90; }
                  
        }
         return LightIntensity;


    } 


}
