using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flycam : MonoBehaviour
{

    Vector3 lookDirection;
    float lookHorizontal;
    float lookVertical;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        //float zoom = Input.GetAxis("Zoom");
        lookHorizontal = lookHorizontal + Input.GetAxis("LookHorizontal");
        lookVertical = lookVertical + Input.GetAxis("LookVertical");
        float speed = 40f;
        transform.Translate(horizontal * speed * Time.deltaTime, 0 , vertical * speed * Time.deltaTime);
        lookDirection = new Vector3(lookVertical, lookHorizontal, 0);
        transform.rotation = Quaternion.Euler(lookDirection);
    }
}
