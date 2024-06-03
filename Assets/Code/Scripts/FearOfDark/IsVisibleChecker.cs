using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsVisibleChecker : MonoBehaviour
{

    private Camera cameraToCheck;

    private Renderer rendererToCheck;

    protected void Awake() {
       rendererToCheck = GetComponent<Renderer>();
    }

    public void OnCameraChanged(Component sender, object data) {

        cameraToCheck = ((GameObject)data).GetComponent<Camera>();

    }

    public bool isVisible() {

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cameraToCheck);

        return GeometryUtility.TestPlanesAABB(planes, rendererToCheck.bounds);


    }
}
