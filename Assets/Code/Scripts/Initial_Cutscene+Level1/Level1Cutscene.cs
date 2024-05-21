using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Cutscene : MonoBehaviour
{
    public GameObject player;
    public GameObject cutsceneCamera;
    public FadeCanvas fadeCanvas;
    public Animator doorAnimator;
    public Animator cameraAnimator;
    

    public const float WAITTIME_DOOROPEN = 1.8f;
    public const float WAITTIME_CAMERAMOVE = 1.5f;
    public const float WAITTIME_CHANGETOPLAYER = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        fadeCanvas.StartFadeOut();

        StartCoroutine(OpenDoor(WAITTIME_DOOROPEN));
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator OpenDoor(float timeInSeconds)
    {
        yield return new WaitForSeconds(timeInSeconds);
        doorAnimator.SetTrigger("doorOpen");

        yield return new WaitForSeconds(WAITTIME_CAMERAMOVE);
        cameraAnimator.SetTrigger("cameraMove");

        yield return new WaitForSeconds(WAITTIME_CHANGETOPLAYER);

        cutsceneCamera.SetActive(false);
        player.SetActive(true);

    }

}