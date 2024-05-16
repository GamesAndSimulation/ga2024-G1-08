using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LookingAtTarget : MonoBehaviour
{

    /// <summary>
    /// The transform of the object this entity will be loocked at 
    /// </summary>
    public Transform target;


    // Update is called once per frame
    void Update()
    {
        Vector3 lookAtOrientation = Quaternion.LookRotation(target.position - transform.position).eulerAngles;


        //this.transform.LookAt(targetCamera.transform);

        //this.transform.Rotate(0, lookAtOrientation.y - this.transform.eulerAngles.y, lookAtOrientation.z - this.transform.eulerAngles.z);
        this.transform.Rotate(0, lookAtOrientation.y - this.transform.eulerAngles.y, 0);


    }
}
