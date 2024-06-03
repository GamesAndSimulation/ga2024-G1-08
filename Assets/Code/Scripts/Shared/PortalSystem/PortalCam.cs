using System;
using UnityEngine;

[Serializable]
public class PortalCam : MonoBehaviour
{
    public Transform playerCam;
    public Transform thisPortal;
    public Transform otherPortal;

    public bool mazeMode;

    void LateUpdate()
    {
        if (mazeMode)
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
        else
        {
            Vector3 playerOffset = playerCam.position - otherPortal.position;
            transform.position = thisPortal.position + playerOffset;

            float angularDifferenceBetweenPortalRotations = Quaternion.Angle(thisPortal.rotation, otherPortal.rotation);

            Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
            Vector3 newCameraDirection = portalRotationalDifference * playerCam.forward;
            transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);

        }

    }
}
