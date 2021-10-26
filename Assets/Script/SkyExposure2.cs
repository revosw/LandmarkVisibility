using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyExposure2 : MonoBehaviour
{
     public Vector3 rayOrigin;
     public float SkyExposurePercentage;
     //public int numOfPixel;
     //public int numOfSky;
     //public Camera cam;
     public GameObject[] myBuilding;

     
     private float ScanTime;
     private float NextScanTime;

     Color skyColor= new Color(95f/255f,158f/255f,160f/255f);
     Color buildingColor= new Color(255f/255f,248f/255f,220f/255f);


    void Update(){
        if(Input.GetMouseButtonDown(0)){                        
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
　　        Vector3 mousePosOnScreen = Input.mousePosition;
　　        mousePosOnScreen.z = screenPos.z; 
　　        Vector3 mousePosInWorld = Camera.main.ScreenToWorldPoint(mousePosOnScreen);
            rayOrigin= mousePosInWorld;
            print(rayOrigin);
            StartCoroutine(CalculateSkyExposure());
        }
    }



   IEnumerator CalculateSkyExposure(){
       int counter_sky=0;
       int counter_total=0;
       int i,j;
        for (i=30;i<=150;i++){
            for(j=60;j>=-60;j--){
                counter_total++;
                //Vector3 rayDirection=new Vector3(Mathf.Cos(i*Mathf.PI/180),Mathf.Sin(i*Mathf.PI/180)*Mathf.Cos(j*Mathf.PI/180),Mathf.Sin(i*Mathf.PI/180)*Mathf.Sin(j*Mathf.PI/180));
                Vector3 rayDirection=new Vector3(1,2,3);
                Vector3 rayOrigin=new Vector3();
                Ray ray=new Ray(rayOrigin,rayDirection);
                //Debug.DrawRay(rayOrigin,transform.up);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, Mathf.Infinity))  
                   {  
                       counter_sky++; 
                        // 在場景檢視中繪製射線  
                       //Debug.DrawLine(ray.origin, hit.point, Color.red); 
                   }  
            }
        }
    SkyExposurePercentage=(1-counter_sky/counter_total)*100;
        
    }

  void Start(){


    }






}