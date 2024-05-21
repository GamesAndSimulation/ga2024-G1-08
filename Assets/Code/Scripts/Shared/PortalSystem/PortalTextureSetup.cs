using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTextureSetup : MonoBehaviour
{

    public Camera cam;
    public Material camMat;

    void Start()
    {
        
        if(cam.targetTexture != null)
        {
            cam.targetTexture.Release();
        } 

        cam.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        camMat.mainTexture = cam.targetTexture;

    }

    
}
