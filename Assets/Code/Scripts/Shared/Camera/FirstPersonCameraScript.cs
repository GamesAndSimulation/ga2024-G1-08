using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Rendering.PostProcessing;

public class FirstPersonCameraScript : CameraScript
{


    public Transform playerObject;
    public Transform head;



    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    public override void rotateCamera(float mouseX, float mouseY) {

        base.rotateCamera(mouseX, mouseY);

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        playerObject.rotation = Quaternion.Euler(0, yRotation, 0);

    }

}
