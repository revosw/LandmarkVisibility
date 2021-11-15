using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFrequencyHeatmap : MonoBehaviour
{

    [Range(1,600)] [SerializeField] int resolution = 3;
    [SerializeField] GameObject plane;
    [SerializeField] GameObject directionalLight;
    Texture2D tex;
    Renderer planeRenderer;
    Vector3[,] rayOrigins;
    Vector3[,] changeFrequencyMap;
    // Start is called before the first frame update
    void Start()
    {
        // Texture that shows the heatmap on the park
        tex = new Texture2D(resolution, resolution);
        // We set filter mode to point to get sharp edges
        tex.filterMode = FilterMode.Point;
        // In order to apply the texture, we need to reference the material of the plane
        planeRenderer = plane.gameObject.GetComponent<Renderer>();

        // Initialize positions of rays
        rayOrigins = new Vector3[resolution,resolution];
        for (int i = 0; i < resolution; i++)
        {
            for(int j = 0; j < resolution; j++)
            {
                Vector3 offset = new Vector3(i * plane.transform.localScale.x * 10/resolution, 0, j * plane.transform.localScale.z * 10/resolution);
                rayOrigins[i, j] = gameObject.transform.position + offset;
            }
        }
    }

    private void FixedUpdate()
    {
        
        // If the sun is facing UP towards the sky, that means it's night, so we don't have
        // to calculate anything
        if (Vector3.Dot(directionalLight.transform.forward, Vector3.up) > 0) return;
        
        // In every ray position, cast a ray
        for (int i = 0; i < resolution; i++)
        {
            for (int j = 0; j < resolution; j++)
            {
                Debug.DrawRay(rayOrigins[i, j], -directionalLight.transform.forward * 100, Color.white);

                // Pixels are originally white...
                tex.SetPixel(i, j, Color.white);
                
                RaycastHit hit;
                if (Physics.Raycast(rayOrigins[i, j], -directionalLight.transform.forward, out hit))
                {
                    // ...however, if they hit something, turn them black as they are in a shadow
                    tex.SetPixel(i, j, Color.black);
                }
            }
        }

        tex.Apply();
        planeRenderer.material.mainTexture = tex;

    }
}
