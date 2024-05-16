using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainSystemScript : MonoBehaviour
{
    public CaptionsScript captionsScript;
    public GameObject captions;
    public GameObject image;
    public FadeCanvas fadeCanvas;
    public Animator lampSwitchAnimator;
    public Animator cameraAnimator;
    public Material lampMaterial;
    public Light lampLight;
    public GameObject particleSystem;
    public AudioSource lightSwitchAudio;
    public float waitTime_LightSwitch = 0.5f;
    public float waitTime_DialogueStart = 4.0f;
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
        lampSwitchAnimator.SetTrigger("hasBeenClicked");

        StartCoroutine(TurnOffLight(waitTime_LightSwitch));

    }

    IEnumerator TurnOffLight(float timeInSeconds)
    {
        lightSwitchAudio.PlayDelayed(timeInSeconds - 0.1f);
        yield return new WaitForSeconds(timeInSeconds);
        lampMaterial.SetColor("_EmissionColor", Color.black);
        lampLight.enabled = false;
        cameraAnimator.SetTrigger("isCameraMoving");
        StartCoroutine(DialogueStart(waitTime_DialogueStart));

    }

    IEnumerator DialogueStart(float timeInSeconds)
    {
        yield return new WaitForSeconds(timeInSeconds);
        captions.SetActive(true);
        image.SetActive(true);

        foreach (var caption in captionsScript.captions)
        {
            captions.GetComponent<TextMeshProUGUI>().text = caption.text;
            yield return new WaitForSeconds(caption.time);
        }

        captions.SetActive(false);
        image.SetActive(false);
        fadeCanvas.QuickFadeIn();
    }


}
