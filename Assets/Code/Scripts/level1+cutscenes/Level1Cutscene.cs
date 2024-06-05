using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Cutscene : MonoBehaviour
{
    public GameObject player;
    public GameObject cutsceneCamera;
    public GameObject door;
    public FadeCanvas fadeCanvas;
    public Animator doorAnimator;
    public Animator cameraAnimator;
    public AudioSource doorOpen;

    public const float WAITTIME_DOOROPEN = 1.0f;
    public const float WAITTIME_DOORSOUND = 0.2f;
    public const float WAITTIME_CAMERAMOVE = 3.0f;
    public const float WAITTIME_CHANGETOPLAYER = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        fadeCanvas.StartFadeOut();
        StartCoroutine(OpenDoor());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator OpenDoor()
    {
        yield return new WaitForSeconds(WAITTIME_CAMERAMOVE);
        cameraAnimator.SetTrigger("cameraMove");

        yield return new WaitForSeconds(WAITTIME_CHANGETOPLAYER);
        cutsceneCamera.SetActive(false);
        player.SetActive(true);
        door.SetActive(true);

        yield return new WaitForSeconds(WAITTIME_DOOROPEN);
        doorAnimator.SetTrigger("doorOpen");

        yield return new WaitForSeconds(WAITTIME_DOORSOUND);
        doorOpen.Play();

    }

}