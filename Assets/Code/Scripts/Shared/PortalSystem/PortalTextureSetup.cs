using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTextureSetup : MonoBehaviour
{

    public Camera cam;
    public Material camMat;

    void Start()
    {

        Setup();

    }

    
    public void Setup()
    {
        if (cam.targetTexture != null)
        {
            cam.targetTexture.Release();
        }

        cam.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        camMat = new Material(Shader.Find("Unlit/ScreenCutoutShader"));
        camMat.mainTexture = cam.targetTexture;
    }

}
