using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Rendering.PostProcessing;

public class FreeMovingCamera : MonoBehaviour
{

    public float sensX = 400;
    public float sensY = 400;

    public Transform head;

    float xRotation;
    float yRotation;


    private void OnEnable() {

        Debug.Log("This object: " + gameObject.name);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        this.transform.position = head.position;
        this.transform.parent = null;

        this.transform.rotation = Quaternion.identity;

    }

    private void OnDisable() {

        this.transform.parent = head.transform;

    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;
        
        yRotation += mouseX;
        xRotation -= mouseY;

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

    }

    public float getCurrentFOV()
    {
        return GetComponent<Camera>().fieldOfView;
    }


}
