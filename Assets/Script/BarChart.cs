using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class BarChart : MonoBehaviour
{
    private Camera cam;
    public GameObject barCube;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickPosition = Vector3.zero;
            if (getMousePosition(out clickPosition))
            {
                Instantiate(barCube, clickPosition, Quaternion.identity);
            }
        }
    }

    bool getMousePosition(out Vector3 clickPosition)
    {
        //Debug.Log($"{Input.mousePosition.x} {Input.mousePosition.y}");
        clickPosition = Vector3.zero;
        if (clickedInsideButtonGroup()) return false;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            clickPosition = hit.point;
            return true;
        }
        return false;
    }

    bool clickedInsideButtonGroup()
    {
        // Left button group
        if (Input.mousePosition.y < 133 && Input.mousePosition.x < 165)
        {
            return true;
        }
        // Right button group
        else if (Input.mousePosition.y < 72 && Input.mousePosition.x > Screen.width - 165)
        {
            return true;
        }
        return false;
    }
}
