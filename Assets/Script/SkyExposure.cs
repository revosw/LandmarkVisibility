using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyExposure : MonoBehaviour
{
     public Vector3 camPosition;
     public float SkyExposurePercentage;
     //public int numOfPixel;
     //public int numOfSky;
     public Camera cam;
     public GameObject[] myBuilding;

     
     private float ScanTime;
     private float NextScanTime;

     Color skyColor= new Color(95f/255f,158f/255f,160f/255f);
     Color buildingColor= new Color(255f/255f,248f/255f,220f/255f);

    
    void Start(){
        transform.position=camPosition;
        transform.eulerAngles= new Vector3(-90,0,0);

        cam = GetComponent<Camera>();
        cam.clearFlags = CameraClearFlags.SolidColor;

        myBuilding= GameObject.FindGameObjectsWithTag("Building");

        ScanTime=3.0f;


        cam.backgroundColor = skyColor;

        foreach (GameObject building in myBuilding)
        {
            building.GetComponent<Renderer>().material.SetColor("_Color",buildingColor);
        }

    }


    void Update(){


        NextScanTime+=Time.deltaTime;
        if(NextScanTime>=ScanTime){
            NextScanTime=0;
            StartCoroutine(CaptureScreenShot());
        }
    }

    IEnumerator CaptureScreenShot(){



        yield return new WaitForEndOfFrame();
        int numOfPixel=0;
        int numOfSky=0;

        Texture2D m_texture=new Texture2D(Screen.width,Screen.height,TextureFormat.RGB24, false);
        m_texture.ReadPixels(new Rect(0,0,Screen.width,Screen.height),0,0);
        m_texture.Apply();
        for (int a=0;a<=Screen.width;a++){
            for (int b=0;b<=Screen.height;b++){
                 numOfPixel++;
                Color colorInShot = m_texture.GetPixel(a,b);
                if(colorInShot.ToString()==skyColor.ToString()){
                    numOfSky++;
                }
            }
        }

        SkyExposurePercentage= numOfSky*100.0f/numOfPixel;
        
    }



}
