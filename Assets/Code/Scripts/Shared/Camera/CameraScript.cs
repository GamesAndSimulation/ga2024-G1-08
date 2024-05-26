using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraScript : MonoBehaviour
{

    public float sensX = 400;
    public float sensY = 400;

    protected float xRotation;
    protected float yRotation;


    public virtual void rotateCamera(float mouseX, float mouseY) {

        yRotation += mouseX * sensX * Time.deltaTime;
        xRotation -= mouseY * sensY * Time.deltaTime;


    }
}
