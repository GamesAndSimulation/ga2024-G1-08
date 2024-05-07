using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHolderScript : MonoBehaviour
{

    public Transform cameraPosition;

    //lock camera to holder
    void Update()
    {
        transform.position = cameraPosition.position;
    }

}
