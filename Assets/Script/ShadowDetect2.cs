using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ShadowDetect2 : MonoBehaviour
{
    //variables' names
    //[SerializeField] private Vector3 position;
    [SerializeField] private int shadow_counter;
    private Vector3[,] positionMatrix;
    Light light;
    Material mMaterial;
    MeshRenderer mMeshRenderer;
    float[] mPoints;
    int mHitCount;
    float width;
    float height;


    void Start()
    {
        mMeshRenderer = GetComponent<MeshRenderer>();
        mMaterial = mMeshRenderer.material;

        // Generate grid of ray origins
        GenerateRayMatrix(8.0f);
    }

    void Awake()
    {
        light = FindObjectOfType<Light>();
    }

    bool IsInShadow(Light light, Vector3 target_position)
    {
        RaycastHit hit;
        Ray ray = new Ray(target_position, -light.transform.forward);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Default")))
        {
            return false;
        }
        return true;
    }




    void GenerateRayMatrix(float resolution)
    {
        GameObject obj = GameObject.Find("Plane001");

        Vector3[] verts = obj.GetComponent<MeshFilter>().sharedMesh.vertices;

        int counter_width = 0;
        int counter_height = 0;
        float x1 = float.MaxValue;
        float x2 = 0;
        float z1 = float.MaxValue;
        float z2 = 0;
        foreach (Vector3 vert in verts)
        {
            if (vert.x < x1) { x1 = vert.x; }
            if (vert.x > x2) { x2 = vert.x; }
            if (vert.z < z1) { z1 = vert.y; }
            if (vert.z > z2) { z2 = vert.y; }
        }

        Vector2 size = new Vector2(Mathf.Abs(x1 - x2), Mathf.Abs(z1 - z2));

        width = size.x;
        height = size.y * 2;

        int num_x = Convert.ToInt32(Math.Ceiling(width / resolution));
        int num_y = Convert.ToInt32(Math.Ceiling(height / resolution));
        positionMatrix = new Vector3[num_x, num_y];

        // Max buffer size is 1024.
        // Our data has a stride of 3.
        // 1023 is the highest number divisible
        // by 3 and lower than 1024.
        mPoints = new float[1023]; //32 point

        mHitCount = 0;

        for (float x = -(size.x / 2); x <= size.x / 2; x += resolution)
        {

            for (float z = -(size.y); z <= size.y; z += resolution)
            {
                positionMatrix[counter_width, counter_height] = new Vector3(x, 0, z);
                counter_height++;
            }
            counter_height = 0;
            counter_width++;
        }

    }


    public void addHitPoint()
    {
        mMaterial.SetFloatArray("_Hits", mPoints);
        mMaterial.SetInt("_HitCount", mHitCount);
    }

    public void startShadowDetect()
    {
        if (light == null) return;

        mHitCount = 0;
        for (int t = 0; t < positionMatrix.GetLength(0); t++)
        {
            for (int s = 0; s < positionMatrix.GetLength(1); s++)
            {
                Vector3 position = positionMatrix[t, s];
                shadow_counter = 0;

                for (int i = 0; i < 180; ++i)
                {
                    light.transform.localEulerAngles = new Vector3(i, transform.localEulerAngles.y, transform.localEulerAngles.z);
                    // Cast ray to see if this position is in shadow
                    if (IsInShadow(light, position))
                    {
                        ++shadow_counter;
                    }
                }

                // Set shader buffer data
                mPoints[mHitCount * 3] = position.x / width;
                mPoints[mHitCount * 3 + 1] = position.z / height;
                mPoints[(t + s) * 3 + 2] = shadow_counter;

                mHitCount++;
            }
        }

        addHitPoint();
    }
}