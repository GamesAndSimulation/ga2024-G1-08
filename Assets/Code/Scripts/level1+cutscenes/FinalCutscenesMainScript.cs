using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCutscenesMainScript : MonoBehaviour
{
    public Animator cameraAnimator;
    public FadeCanvas fadeCanvas;
    public GameObject credits;
    public Level1DogStateHandler level1DogStateHandler;

    public const string CAMERAMOVEMENT = "CameraMoving";

    public const float WAITTIME_CAMERAMOVEMENT = 3.0f;
    public float WAITTIME_FADEOUTSCENE = 8.0f;
    public const float WAITTIME_FADEINCREDITS = 2.0f;



    // Start is called before the first frame update
    void Start()
    {
        fadeCanvas.StartFadeOut();
        level1DogStateHandler.StartSniffingUp();
        StartCoroutine(CameraMove());
        

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator CameraMove()
    {
        yield return new WaitForSeconds(WAITTIME_CAMERAMOVEMENT);

        cameraAnimator.SetTrigger(CAMERAMOVEMENT);

        StartCoroutine(RollCredits());
    }

    IEnumerator RollCredits()
    {
        yield return new WaitForSeconds(WAITTIME_FADEOUTSCENE);
        fadeCanvas.StartFadeIn();

        yield return new WaitForSeconds(WAITTIME_FADEINCREDITS);
        credits.SetActive(true);
        fadeCanvas.StartFadeOut();

    }
}
