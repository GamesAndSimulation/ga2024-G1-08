using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Rendering.PostProcessing;

public class FreeMovingCamera : CameraScript
{



    [SerializeField] private Transform head;


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

    public override void rotateCamera(float mouseX, float mouseY) {

        base.rotateCamera(mouseX, mouseY);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

    }


}
