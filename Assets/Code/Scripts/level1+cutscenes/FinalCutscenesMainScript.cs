using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalCutscenesMainScript : MonoBehaviour
{
    public Animator cameraAnimator;
    public FadeCanvas fadeCanvas;
    public GameObject credits;
    public Level1DogStateHandler level1DogStateHandler;
    public AudioSource ambience;
    public AudioVolume audioVolume;

    public const string CAMERAMOVEMENT = "CameraMoving";

    public const float WAITTIME_CAMERAMOVEMENT = 3.0f;
    public float WAITTIME_FADEOUTSCENE = 8.0f;
    public const float WAITTIME_SWITCHSCENES = 2.0f;
    public const float AUDIOFADEIN = 5.0f;
    public const float AUDIOFADEOUT = 2.0f;
    public string INITIAL_SCENE = "InitialCutscene";



    // Start is called before the first frame update
    void Start()
    {
        fadeCanvas.StartFadeOut();
       
        if(level1DogStateHandler != null )
            level1DogStateHandler.StartSniffingUp();

        StartCoroutine(audioVolume.IncreaseVolume(ambience, AUDIOFADEIN));

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

        StartCoroutine(EndScene());
    }

    IEnumerator EndScene()
    {
        yield return new WaitForSeconds(WAITTIME_FADEOUTSCENE);
        StartCoroutine(audioVolume.ReduceVolume(ambience, AUDIOFADEOUT));

        fadeCanvas.StartFadeIn();
       

        yield return new WaitForSeconds(WAITTIME_SWITCHSCENES);
        SceneManager.LoadScene(INITIAL_SCENE);

    }
}
