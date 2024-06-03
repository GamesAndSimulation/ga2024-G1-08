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
    public GameObject particleSystem;
    public AudioSource lightSwitchAudio;
    public AudioSource music;

    public const float WAITTIME_LIGHTSWITCH = 0.5f;
    public const float WAITTIME_DIALOGUESTART = 4.0f;
    public const float WAITTIME_TURNOFFCAPTIONS = 6.0f;
    public const float WAITTIME_FADE = 1.0f;
    public const float WAITTIME_SWITCHSCENES = 2.0f;
    public const float AUDIOFADETIME = 8.5f;

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
        particleSystem.SetActive(false);
        lampSwitchAnimator.SetTrigger("hasBeenClicked");
        music.Play();
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

        captionsScript.PlayCaptions();

        StartCoroutine(ReduceAudioVolume());

        yield return new WaitForSeconds(WAITTIME_TURNOFFCAPTIONS);
        captions.SetActive(false);

        
        StartCoroutine(SwitchScenes());
        
    }

    IEnumerator SwitchScenes()
    {

        yield return new WaitForSeconds(WAITTIME_FADE);
        fadeCanvas.QuickFadeIn();

        yield return new WaitForSeconds(WAITTIME_SWITCHSCENES);
        SceneManager.LoadScene(LEVEL1);

    }

    IEnumerator ReduceAudioVolume()
    {
        float startVolume = music.volume;

        while (music.volume > 0)
        {
            music.volume -= startVolume * Time.deltaTime / AUDIOFADETIME;

            yield return null;
        }

    }
}
