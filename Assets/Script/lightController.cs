using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightController : MonoBehaviour
{

    public GameObject parent;
    public float rotationSpeed = 100.0F;
    public GameObject target;



    // Start is called before the first frame update
    void Start()
    {
        GridPlane gridPlane = new GridPlane(4, 2, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        
            transform.RotateAround(parent.transform.position, Vector3.up , rotationSpeed);

            Vector3 fromPosition = transform.position;
            Vector3 toPosition = target.transform.position;
            Vector3 direction = toPosition - fromPosition;


        if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity))
            {
                //Debug.DrawRay(transform.position, direction * hit.distance, Color.yellow);

            }
            else
            {
                //Debug.DrawRay(transform.position, direction * 1000, Color.white);
            }
        
        
        }
}
