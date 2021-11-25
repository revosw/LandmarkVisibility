using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BarChart : MonoBehaviour
{

    public Terrain terr; // terrain to modify
    public float LightAngle;
    public  int Gridsize;

    private int hmWidth; // heightmap width
    private int hmHeight; // heightmap height
    private List<Light> Lights;
    private Vector3[,] positionMatrix;
    private float height;
    private Light light;
    private float width;
    private int total_num;


    void Awake(){
        PositionMatrix(Gridsize);
        Lights = new List<Light>(FindObjectsOfType<Light>()); 
        light = Lights[1];


        terr = Terrain.activeTerrain;
        hmWidth = terr.terrainData.heightmapResolution;
        hmHeight = terr.terrainData.heightmapResolution;
    }


     void Update()
     { 
        if(Input.GetMouseButtonDown(0)){
            if (light == null) { print("no light"); return; }

            //for (int m = 40; m < 180; ++m){
                //WaitForSeconds wait = new WaitForSeconds( 0.2f ) ;
                //float m=80f;
                light.transform.localEulerAngles = new Vector3(LightAngle, transform.localEulerAngles.y, transform.localEulerAngles.z);

                for (int t=0;t<positionMatrix.GetLength(0);t++){
                    for (int s=0;s<positionMatrix.GetLength(1);s++){

                        Vector3 position = positionMatrix[t,s];

                        Vector3 tempCoord = (position - terr.gameObject.transform.position);
                        Vector3 coord;
                        coord.x = tempCoord.x / terr.terrainData.size.x;
                        coord.y = tempCoord.y / terr.terrainData.size.y;
                        coord.z = tempCoord.z / terr.terrainData.size.z;

        // get the position of the terrain heightmap where this game object is
                        float posXInTerrain = coord.x * hmWidth;
                        float posYInTerrain = coord.z * hmHeight;

                        // we set an offset so that all the raising terrain is under this game object
                        float offset = Gridsize / 2;

// get the heights of the terrain under this game object
                        float[,] heights = terr.terrainData.GetHeights(Convert.ToInt32(posXInTerrain - offset), Convert.ToInt32(posYInTerrain - offset), Gridsize, Gridsize);

                        if (!IsInDirectionalLightShadow(light, position)){
                            float angle = (float)(LightAngle/180);
                            var heightScale = 1.0f / terr.terrainData.size.y;
                            for (int i = 0; i < Gridsize / 2; i++){
                                for (int j = 0; j < Gridsize / 2; j++)
                                        {heights[i, j] = 5*angle*heightScale;}
                            }
                        }
                        else{
                            for (int i = 0; i < Gridsize / 2; i++){
                                for (int j = 0; j < Gridsize / 2; j++)
                                    {heights[i, j] = 0;}
                            }
                        }
                                // set the new height
                            terr.terrainData.SetHeights(Convert.ToInt32(posXInTerrain - offset), Convert.ToInt32(posYInTerrain - offset), heights);

                    }}

            //}
        }
     }




    bool IsInDirectionalLightShadow(Light light, Vector3 target_position)
        {
            RaycastHit hit;
            Ray ray = new Ray(target_position, -light.transform.forward);
            //Debug.DrawRay(ray.origin, ray.direction, Color.red);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1<<LayerMask.NameToLayer("Default")))
            {
                return true;
            }
            return false;
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
}
