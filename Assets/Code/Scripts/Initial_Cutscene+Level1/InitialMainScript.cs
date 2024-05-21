using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialMainScript : MonoBehaviour
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
    public float WAITTIME_LIGHTSWITCH = 0.5f;
    public float WAITTIME_DIALOGUESTART = 4.0f;
    public float WAITTIME_SWITCHSCENES = 2.0f;
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
        image.SetActive(true);

        foreach (var caption in captionsScript.captions)
        {
            captions.GetComponent<TextMeshProUGUI>().text = caption.text;
            yield return new WaitForSeconds(caption.time);
        }

        captions.SetActive(false);
        image.SetActive(false);
        fadeCanvas.QuickFadeIn();

        StartCoroutine(SwitchScenes());
    }

    IEnumerator SwitchScenes()
    {
        yield return new WaitForSeconds(WAITTIME_SWITCHSCENES);

        SceneManager.LoadScene(LEVEL1);

    }
}
