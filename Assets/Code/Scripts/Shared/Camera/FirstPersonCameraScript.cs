using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Rendering.PostProcessing;

public class FirstPersonCameraScript : MonoBehaviour
{

    public float sensX;
    public float sensY;

    public Transform playerObject;
    public Transform head;

    float xRotation;
    float yRotation;


    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;
        
        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        playerObject.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public float getCurrentFOV()
    {
        return GetComponent<Camera>().fieldOfView;
    }


}
