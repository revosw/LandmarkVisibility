using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPlane
{
    private int width;
    private int height;
    private float cellSize;
    private int[,] gridArray;

    public GridPlane(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new int[width, height];

        for(int x = 0; x < gridArray.GetLength(0); x++)
        {
            for(int y = 0; y < gridArray.GetLength(1); y++)
            {
                //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.red);
                //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.red);
            }
        }
        //Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.red);
        //Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.red);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }

}

