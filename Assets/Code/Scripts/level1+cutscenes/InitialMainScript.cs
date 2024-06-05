using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialMainScript : MonoBehaviour
{
    public CaptionsScript captionsScript;
    public GameObject captions;
    public FadeCanvas ui;
    public FadeCanvas fadeCanvas;
    public Animator lampSwitchAnimator;
    public Animator cameraAnimator;
    public Material lampMaterial;
    public Light lampLight;
    public GameObject particleSystemObject;
    public AudioSource lightSwitchAudio;
    public AudioSource music;
    public AudioVolume audioVolume;

    public const float WAITTIME_LIGHTSWITCH = 0.5f;
    public const float WAITTIME_DIALOGUESTART = 4.0f;
    public const float WAITTIME_TURNOFFCAPTIONS = 6.0f;
    public const float WAITTIME_FADE = 1.0f;
    public const float WAITTIME_SWITCHSCENES = 2.0f;
    public const float AUDIOINFADETIME = 2.0f;
    public const float AUDIOOUTFADETIME = 8.5f;

    public string LEVEL1 = "Level1";
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
        particleSystemObject.SetActive(false);
        lampSwitchAnimator.SetTrigger("hasBeenClicked");
        StartCoroutine(audioVolume.IncreaseVolume(music, AUDIOINFADETIME));
        ui.StartFadeOut();
        StartCoroutine(TurnOffLight());

    }

    IEnumerator TurnOffLight()
    {
        lightSwitchAudio.PlayDelayed(WAITTIME_LIGHTSWITCH - 0.1f);
        yield return new WaitForSeconds(WAITTIME_LIGHTSWITCH);
        lampMaterial.SetColor("_EmissionColor", Color.black);
        lampLight.enabled = false;
        cameraAnimator.SetTrigger("isCameraMoving");
        StartCoroutine(DialogueStart());

    }

    IEnumerator DialogueStart()
    {
        yield return new WaitForSeconds(WAITTIME_DIALOGUESTART);

        captions.SetActive(true);
        StartCoroutine(captionsScript.PlayCaptions());

        StartCoroutine(audioVolume.ReduceVolume(music, AUDIOOUTFADETIME));

        yield return new WaitForSeconds(WAITTIME_TURNOFFCAPTIONS);
        captions.SetActive(false);

        
        StartCoroutine(SwitchScenes());

        
    }

    IEnumerator SwitchScenes()
    {

        yield return new WaitForSeconds(WAITTIME_FADE);
        fadeCanvas.QuickFadeIn();

        yield return new WaitForSeconds(WAITTIME_SWITCHSCENES);
        LevelsManager.instance.transitionToLevel1();

    }

}
