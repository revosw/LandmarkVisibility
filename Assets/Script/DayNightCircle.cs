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

    private bool activ;
    private bool isPushed;
    private bool run;

    void Start()
    {
        light = GetComponent<Light>();
        isPushed = false;
    }

    public IEnumerator cycle(){
        while(run){
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

    public void startDayNight()
    {
        if(isPushed == false)
        {
            Debug.Log("trykket");
            activ = true;
            isPushed = true;
            run = true;
            StartCoroutine(cycle());
        }
        else
        {
            activ = false;
            isPushed = false;
            run = false;
        }
    }
}