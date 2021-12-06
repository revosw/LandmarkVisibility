using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ChangeFrequencyHeatmap : MonoBehaviour
{
    // Object references
    [SerializeField] GameObject plane;
    [SerializeField] GameObject directionalLight;
    [SerializeField] GameObject inputPattern;

    // Plane
    Texture2D tex;
    Renderer planeRenderer;
    [Range(1, 30)] [SerializeField] int resolution = 8;


    Vector3[,] rayOrigins;

    bool stopRecordingShadows;
    List<bool[,]> allShadowReadings;
    bool[,] previousShadows;
    bool[,] currentShadows;
    int[,] changeFrequencyMap;

    bool firstDay = true;

    void Start()
    {
        previousShadows = new bool[resolution, resolution];
        currentShadows = new bool[resolution, resolution];
        changeFrequencyMap = new int[resolution, resolution];
        allShadowReadings = new List<bool[,]>();
        // Texture that shows the heatmap on the park
        tex = new Texture2D(resolution, resolution);
        // We set filter mode to point to get sharp edges
        tex.filterMode = FilterMode.Point;
        // In order to apply the texture, we need to reference the material of the plane
        planeRenderer = plane.gameObject.GetComponent<Renderer>();

        // Initialize positions of rays
        rayOrigins = new Vector3[resolution, resolution];
        for (int i = 0; i < resolution; i++)
        {
            for (int j = 0; j < resolution; j++)
            {
                Vector3 offset = new Vector3(i * plane.transform.localScale.x * 10 / resolution, 0, j * plane.transform.localScale.z * 10 / resolution);
                rayOrigins[i, j] = gameObject.transform.position + offset;
            }
        }
    }

    public void CalculateCFH()
    {
        var shadowReadingsCopy = allShadowReadings.ToArray();
        int[,] cfh = new int[resolution, resolution];

        var pattern = inputPattern.GetComponent<Text>().text;
        if (pattern.Length < 3) return;

        for (int rec = 0; rec < shadowReadingsCopy.Length - 2; rec++)
        {
            //bool[,] cellMatchesPattern = new bool[resolution, resolution];
            for (int i = 0; i < resolution; i++)
            {
                for (int j = 0; j < resolution; j++)
                {
                    var firstReading = shadowReadingsCopy[rec][i, j] ? 1 : 0;
                    var secondReading = shadowReadingsCopy[rec + 1][i, j] ? 1 : 0;
                    var thirdReading = shadowReadingsCopy[rec + 2][i, j] ? 1 : 0;
                    if ($"{firstReading}{secondReading}{thirdReading}" == pattern)
                    {
                        //cellMatchesPattern[i, j] = true;
                        cfh[i, j]++;
                    }
                }
            }
            //shadowReadingsCopy[rec] = cellMatchesPattern;
        }

        DrawChangeFrequencyMapTexture(NormalizeChangeFrequencyMap(cfh));
    }

    private void FixedUpdate()
    {
        // If the sun is facing UP towards the sky, that means it's night, so we don't have
        // to calculate shadows
        if (Vector3.Dot(directionalLight.transform.forward, Vector3.up) > 0)
        {
            firstDay = false;
            if (allShadowReadings.Count > 0)
            {
                stopRecordingShadows = true;
            }
            return;
        }
        // We want a consistent reading, so we skip the entire first day in case the sun starts at midday on day 1
        if (firstDay) return;
        // If we have already recorded shadows for one day, there's no more useful data to gather
        if (stopRecordingShadows) return;


        //------------------------------------
        // REST OF FUNCTION ONLY RUNS ON DAY 2

        currentShadows = RecordShadows();
        allShadowReadings.Add(currentShadows);
        IncrementShadowChangeCounter();
        previousShadows = currentShadows;

        // The change frequency heatmap is now updated
        var scaledHeatMap = NormalizeChangeFrequencyMap(changeFrequencyMap);
        //PrintCFH(scaledHeatMap);
        //PrintCFH(changeFrequencyMap);
        DrawChangeFrequencyMapTexture(scaledHeatMap);
    }

    private bool[,] RecordShadows()
    {
        var shadows = new bool[resolution, resolution];

        for (int i = 0; i < resolution; i++)
        {
            for (int j = 0; j < resolution; j++)
            {
                shadows[i, j] = Physics.Raycast(rayOrigins[i, j], -directionalLight.transform.forward);
            }
        }

        return shadows;
    }

    private void IncrementShadowChangeCounter()
    {
        for (int i = 0; i < resolution; i++)
        {
            for (int j = 0; j < resolution; j++)
            {
                if (previousShadows[i, j] != currentShadows[i, j])
                {
                    changeFrequencyMap[i, j]++;
                }
            }
        }
    }

    /// <summary>
    /// Given a change frequency map that might look like this:
    /// 
    /// [  4, 20,  3,
    ///    8,  0,  1,
    ///    5,  2,  0 ]
    ///    
    /// normalize it like this:
    /// 
    /// [ 0.2, 1, 0.15,
    ///   0.4, 0, 0.05,
    ///   0.25, 0.1, 0 ]
    ///   
    /// </summary>
    /// <param name="heatmap">Change frequency heatmap</param>
    /// <returns></returns>
    private float[,] NormalizeChangeFrequencyMap(int[,] heatmap)
    {
        // First find out what the max value is
        float max = 0;
        for (int i = 0; i < resolution; i++)
        {
            for (int j = 0; j < resolution; j++)
            {
                if (heatmap[i, j] > max)
                {
                    max = heatmap[i, j];
                }
            }
        }

        var scaledHeatMap = new float[resolution, resolution];

        // Now we know what the max value is, and we have
        // to scale all the values in the heatmap down
        for (int i = 0; i < resolution; i++)
        {
            for (int j = 0; j < resolution; j++)
            {
                scaledHeatMap[i, j] = heatmap[i, j] / max;
            }
        }

        return scaledHeatMap;
    }

    /// <summary>
    /// Draws the change frequency heatmap
    /// </summary>
    /// <param name="heatmap">Change frequency heatmap</param>
    /// <returns></returns>
    private void DrawChangeFrequencyMapTexture(float[,] heatmap)
    {
        for (int i = 0; i < resolution; i++)
        {
            for (int j = 0; j < resolution; j++)
            {
                tex.SetPixel(i, j, new Color(heatmap[i, j], heatmap[i, j], heatmap[i, j]));
            }
        }

        tex.Apply();
        planeRenderer.material.mainTexture = tex;
    }

    private void PrintCFH(float[,] heatmap)
    {
        string text = "\n";
        for (int i = 0; i < resolution; i++)
        {
            for (int j = 0; j < resolution; j++)
            {
                text += $"{heatmap[i, j]} ";
            }
            text += "\n";
        }
    }

    private void PrintCFH(int[,] heatmap)
    {
        string text = "\n";
        for (int i = 0; i < resolution; i++)
        {
            for (int j = 0; j < resolution; j++)
            {
                text += $"{heatmap[i, j]} ";
            }
            text += "\n";
        }
    }
}
