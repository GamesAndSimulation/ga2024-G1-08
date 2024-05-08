using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSystemScript : MonoBehaviour
{
    public Animator lampSwitchAnimator;
    public Animator cameraAnimator;
    public Material lampMaterial;
    public Light lampLight;
    public GameObject particleSystem;
    public float waitTime = 0.5f;
    public Vector4 emissionColor = new Vector4(0.9471698f, 0.7891425f, 0.112588f, 1f);
    // Start is called before the first frame update
    void Start()
    {
        lampMaterial.SetColor("_EmissionColor", emissionColor);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void switchLampOff(Component sender, object data)
    {
        particleSystem.SetActive(false);
        lampSwitchAnimator.SetBool("hasBeenClicked", true);

        StartCoroutine(TurnOffLight(waitTime));

    }

    IEnumerator TurnOffLight(float timeInSeconds)
    {
        yield return new WaitForSeconds(timeInSeconds);
        lampMaterial.SetColor("_EmissionColor", Color.black);
        lampLight.enabled = false;
        cameraAnimator.SetBool("isCameraMoving", true);
    }
}
