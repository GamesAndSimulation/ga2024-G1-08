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
        Vector3 playerOffsetFromOtherPortal = playerCam.position - otherPortal.position;

        Quaternion portalRotationDifference = thisPortal.rotation * Quaternion.Inverse(otherPortal.rotation);
        Vector3 transformedOffset = portalRotationDifference * playerOffsetFromOtherPortal;

        Vector3 mirroredPosition = thisPortal.position - transformedOffset;

        transform.position = mirroredPosition;

        Vector3 localForward = otherPortal.InverseTransformDirection(playerCam.forward);
        localForward = new Vector3(-localForward.x, localForward.y, -localForward.z); // Mirror horizontally only
        Vector3 newCameraDirection = thisPortal.TransformDirection(localForward);
        transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
    }
}
