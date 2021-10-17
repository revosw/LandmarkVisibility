using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCircle : MonoBehaviour
{
    public float rotateSpeed;
    public float cycleSpeed;

    public float xRotation;
    public float intensity;

    public Light light;

    void Start()
    {
        light = GetComponent<Light>();
        StartCoroutine(cycle());
    }

    public IEnumerator cycle(){
        while(true){
            yield return new WaitForSeconds(cycleSpeed);
            rotate();
        }
    }

    public void rotate()
    {
        transform.Rotate(Vector3.up*rotateSpeed);
        xRotation=transform.rotation.eulerAngles.x;
        light.intensity = (xRotation>150)? 0 : intensity;

    }
}