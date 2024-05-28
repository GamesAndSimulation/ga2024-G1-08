using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCam : MonoBehaviour
{

    public Transform playerCam;
    public Transform thisPortal;
    public Transform otherPortal;

    void LateUpdate()
    {
        Vector3 playerOffset = playerCam.position - otherPortal.position;
        transform.position = thisPortal.position + playerOffset;

        float angularDifferenceBetweenPortalRotations = Quaternion.Angle(thisPortal.rotation, otherPortal.rotation);

        Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
        Vector3 newCameraDirection = portalRotationalDifference * playerCam.forward;
        transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
    
    }
}
