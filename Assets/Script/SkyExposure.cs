using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyExposure : MonoBehaviour
{
    public Camera cam;
    public Vector3 mousePos;
    public float SkyExposurePercentage;

    void Start(){
        cam = GetComponent<Camera>();
    }

//get the position of mouse click
    void Update(){
        if(Input.GetMouseButtonDown(0)){ 

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycast;
            if(Physics.Raycast(ray, out raycast,float.MaxValue, 1<<LayerMask.NameToLayer("Ground"))){
                mousePos=raycast.point;
                StartCoroutine(CalculateSkyExposure());
            }
        }   
    }


//calculate the sky exposure
    IEnumerator CalculateSkyExposure(){
       int counter_sky=0;
       int counter_total=0;
       int i,j;
        for (i=30;i<=150;i++){
            for(j=60;j>=-60;j--){
                counter_total++;
                Vector3 rayDirection=new Vector3(Mathf.Cos(i*Mathf.PI/180),Mathf.Sin(i*Mathf.PI/180)*Mathf.Cos(j*Mathf.PI/180),Mathf.Sin(i*Mathf.PI/180)*Mathf.Sin(j*Mathf.PI/180));
                Ray ray=new Ray(mousePos,rayDirection);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, float.MaxValue, 1<<LayerMask.NameToLayer("Default")))  
                   {  
                       counter_sky++;                        

                   }  
            }

        }

     SkyExposurePercentage=(1-counter_sky*1.0f/counter_total)*100;
     yield return new WaitForSeconds(3f);
        
    }   

}
