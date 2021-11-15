using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class SahdowDetect2 : MonoBehaviour
{
    //variables' names
    //[SerializeField] private Vector3 position;
    [SerializeField] private int shadow_counter;
    [SerializeField] private LayerMask _layers;
//    [SerializeField] 
    private List<Light> Lights;

    private Vector3[,] positionMatrix;

    Material mMaterial;
    MeshRenderer mMeshRenderer;

    float[] mPoints;
    int mHitCount;
    Light light;
    float width;
    float height;
    int total_num;


    void Start()
    { 


        PositionMatrix(5.0f);
        Lights = new List<Light>(FindObjectsOfType<Light>()); 

        mMeshRenderer = GetComponent<MeshRenderer>();
        mMaterial = mMeshRenderer.material;



    }

    void Awake(){
        PositionMatrix(10f);
        Lights = new List<Light>(FindObjectsOfType<Light>()); 
        light = Lights[0];
        //light.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(180,transform.localEulerAngles.y, transform.localEulerAngles.z),1.0f);
    }

     void Update()
     {

        if(Input.GetMouseButtonDown(0)) 
        {
            //Vector3 V3 = position;
            mHitCount=0;   
                
                if (light == null) { return; }


                 for (int t=0;t<positionMatrix.GetLength(0);t++){
                    for (int s=0;s<positionMatrix.GetLength(1);s++){
                        Vector3 position = positionMatrix[t,s];
                        shadow_counter = 0;

                        for (int i = 0; i < 180; ++i)
                        {
                            light.transform.localEulerAngles = new Vector3(i, transform.localEulerAngles.y, transform.localEulerAngles.z);
                            if (IsInDirectionalLightShadow(light, position))
                            {
                                ++shadow_counter;
                            }
                        }

                        
                        mPoints[mHitCount * 3] = position.x/width;
                        mPoints[mHitCount * 3 + 1] = position.z/height;
                        mPoints[(t+s) * 3 + 2] = shadow_counter;
                        mHitCount++;
                        print("Shadow counter in position"+ position[0]+position[1]+position[2] +"is" + shadow_counter);

                    
                    }
                }

           addHitPoint();     
            
        }
    }      

    bool IsInDirectionalLightShadow(Light light, Vector3 target_position)
        {
            RaycastHit hit;
            Ray ray = new Ray(target_position, -light.transform.forward);
            //Debug.DrawRay(ray.origin, ray.direction, Color.red);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1<<LayerMask.NameToLayer("Default")))
            {
                return false;
            }
            return true;
        }




    void PositionMatrix( float res)
    {
        GameObject obj = GameObject.Find("Plane001");
  
        Vector3[] verts = obj.GetComponent<MeshFilter>().sharedMesh.vertices;

        int counter_width=0;
        int counter_height=0;
        float x1 = float.MaxValue;
        float x2 = 0;
        float z1 = float.MaxValue;
        float z2 = 0;
        foreach (Vector3 vert in verts)
        {
            if (vert.x < x1) { x1 = vert.x;}
            if (vert.x > x2) { x2 = vert.x;}
            if (vert.z < z1) { z1 = vert.y;}
            if (vert.z > z2) { z2 = vert.y;}
        }

        Vector2 size = new Vector2(Mathf.Abs(x1 - x2), Mathf.Abs(z1 - z2));

        width = size.x;
        height = size.y*2;

        int num_x=Convert.ToInt32(Math.Ceiling(width/res));
        int num_y=Convert.ToInt32(Math.Ceiling(height/res));
        total_num=num_x*num_y;        
        positionMatrix = new Vector3[num_x,num_y];

        mPoints = new float[total_num * 3]; //32 point 

        mHitCount=0;

        for (float x = -(size.x / 2); x <= size.x / 2; x += res)
        {

            for (float z = -(size.y ); z <= size.y ; z += res)
            {
                 positionMatrix[counter_width,counter_height]=new Vector3(x, 0, z);
                //print(positionMatrix[counter_width,counter_height]);                
                counter_height++;
            }
            counter_height=0;
            counter_width++;
        }

    }
  
     
  public void addHitPoint()
  {
        mMaterial.SetFloatArray("_Hits", mPoints);
    mMaterial.SetInt("_HitCount", mHitCount);

  }

}