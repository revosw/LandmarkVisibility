using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public float rotateSpeed;
    public float cycleSpeed;

    public float xRotation;
    public float intensity;

    public Light light;

    private bool toggled;

    void Start()
    {
        light = GetComponent<Light>();
        //light.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(50,132, 15),1.0f);
    }

    public void toggleDayNightCycle()
    {
        if (!toggled)
        {
            toggled = true;
            StartCoroutine(cycle());
        }
        else
        {
            toggled = false;
        }
    }

    IEnumerator cycle()
    {
        while (toggled)
        {
            yield return new WaitForSeconds(cycleSpeed);
            rotate();
        }
    }

    void rotate()
    {
        Vector3 rotationdirection = new Vector3(1, 0, 0);
        transform.Rotate(rotationdirection * rotateSpeed);
        xRotation = transform.rotation.eulerAngles.x;
        light.intensity = (xRotation > 150) ? 0 : intensity;
    }
}