using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalDecor : MonoBehaviour, Decoration
{
    [SerializeField] private Vector3 basePosition;
    [SerializeField] private Vector3 baseRotation; //baseRotation is always set by the North decor spot

    [SerializeField] public Transform receiver;
    [SerializeField] public Transform otherPortal;

    [Header("Scripts")]
    [SerializeField] public PortalCam portalCam;
    [SerializeField] public PortalTeleport portalTeleport;
    [SerializeField] public PortalTextureSetup portalTextureSetup;

    public void GenObject(Direction dir)
    {
        portalCam = GetComponentInChildren<PortalCam>();
        portalTeleport = GetComponentInChildren<PortalTeleport>();
        portalTextureSetup = GetComponentInChildren<PortalTextureSetup>();

        this.transform.localPosition = basePosition;
        this.transform.localRotation = Quaternion.Euler(baseRotation);

        portalCam.playerCam = GameObject.Find("Main Camera").GetComponent<Camera>().transform;
        portalCam.otherPortal = otherPortal;

        portalTeleport.player = GameObject.Find("Player").transform;
        portalTeleport.receiver = receiver;

        portalTextureSetup.cam = GetComponentInChildren<Camera>();
        portalCam.thisPortal = portalTextureSetup.cam.transform;
        
        portalTextureSetup.Setup();
    }

    public void SetReceiver(Transform receiver)
    {
        this.receiver = receiver;
        portalTeleport.receiver = receiver;
    }

    public void SetOtherPortal(Transform otherPortal)
    {
        this.otherPortal = otherPortal;
        Debug.Log(portalCam);
        portalCam.otherPortal = otherPortal;
    }

    
}
