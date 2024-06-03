using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class PortalDecor : MonoBehaviour, Decoration
{
    [Header("Position and Rotation")]
    [SerializeField] private Vector3 basePosition;
    [SerializeField] private Vector3 baseRotation; // baseRotation is always set by the North decor spot

    [Header("Scripts")]
    [SerializeField] public PortalCam portalCam;
    [SerializeField] public PortalTeleport portalTeleport;
    [SerializeField] public PortalTextureSetup portalTextureSetup;


    public void GenObject(Direction dir)
    {
        transform.localPosition = basePosition;
        transform.localRotation = Quaternion.Euler(baseRotation);
    }

    public void SetPlayer(Transform player)
    {
        portalTeleport.player = player;
    }
    public void SetReceiver(Transform receiver)
    {
        portalTeleport.receiver = receiver;
    }

    public void SetPlayerCam(Transform playerCam)
    {
        portalCam.playerCam = playerCam;
    }

    public void SetThisPortal()
    {
        portalCam.thisPortal = this.transform;
    }

    public void SetOtherPortal(Transform otherPortal)
    {
        portalCam.otherPortal = otherPortal;
    }

    public void SetPortalCam(Camera cam)
    {
        portalTextureSetup.cam = cam;
    }

    public void SetPortalMat(Material mat)
    {
        portalTextureSetup.camMat = mat;
    }

    public void ApplyMatToPlane(Material mat)
    {
        GameObject plane = transform.GetChild(1).gameObject;
        plane.GetComponent<MeshRenderer>().material = mat;
    }

}
