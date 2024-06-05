using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsManager : MonoBehaviour
{

    public static LevelsManager instance { get; private set; }

    [Header("Events")]

    [SerializeField] private GameEvent fodLevelStart;

    private bool canTransitionScene = true;
    


    private void Awake() {

        if (instance == null)
            instance = this;

        else
            Destroy(gameObject);


        DontDestroyOnLoad(this);

    }


    // Start is called before the first frame update
    void Start()
    {
        transitionToInitialScene();
    }

    private int currentLevel = 0;

    public void transitionToInitialCutscene()
    {

        if (canTransitionScene)
        {
            canTransitionScene = false;
            unloadCurrentLevel();
            transitionToInitialScene();

        }
    }


    public void transitionToLevel1() {

        if (canTransitionScene) {
            canTransitionScene = false;
            unloadCurrentLevel();
            transitionToFirstLevel();
        }

    }

    public void transitionToLevel2() {

        if (canTransitionScene) {
            canTransitionScene = false;
            unloadCurrentLevel();
            transitionToSecondLevel();
        
        }

    }

    public void transitionToBadEndingCutscene()
    {

        if (canTransitionScene)
        {
            canTransitionScene = false;
            unloadCurrentLevel();
            transitionToBadEnding();

        }
    }

    public void transitionToGoodEndingCutscene()
    {

        if (canTransitionScene)
        {
            canTransitionScene = false;
            unloadCurrentLevel();
            transitionToGoodEnding();

        }

    }

    private IEnumerator setActiveScene(string sceneName) {

        yield return null; // Wait one frame for the scene to load

        Scene newScene = SceneManager.GetSceneByName(sceneName);

        if (newScene.isLoaded) {
            // Set active scene to apply its lighting settings
            SceneManager.SetActiveScene(newScene);
            canTransitionScene = true;
        }
    }

    private void transitionToInitialScene()
    {
        SceneManager.LoadScene("InitialCutscene", LoadSceneMode.Additive);
        currentLevel = 0;
        StartCoroutine(setActiveScene("InitialCutscene"));
    }

    private void transitionToFirstLevel() {

        SceneManager.LoadScene("Level1", LoadSceneMode.Additive);
        currentLevel = 1;
        StartCoroutine(setActiveScene("Level1"));
    }

    private void transitionToSecondLevel() {

        SceneManager.LoadScene("FearOfDarkScene", LoadSceneMode.Additive);
        currentLevel = 2;
        StartCoroutine(setActiveScene("FearOfDarkScene"));
    }

    private void transitionToBadEnding()
    {

        SceneManager.LoadScene("BadEnding", LoadSceneMode.Additive);
        currentLevel = 5;
        StartCoroutine(setActiveScene("BadEnding"));
    }

    private void transitionToGoodEnding()
    {

        SceneManager.LoadScene("GoodEnding", LoadSceneMode.Additive);
        currentLevel = 6;
        StartCoroutine(setActiveScene("GoodEnding"));
    }



    public void unloadCurrentLevel() {

        switch(currentLevel) {
            case 0:
                SceneManager.UnloadScene("InitialCutscene");
                break;
            case 1:
                SceneManager.UnloadScene("Level1");

                break;

            case 2:
                SceneManager.UnloadScene("FearOfDarkScene"); 
                break;
            case 5:
                SceneManager.UnloadScene("BadEnding");
                break;
            case 6:
                SceneManager.UnloadScene("GoodEnding");
                break;

        }

    }


    public void transitionToFOD() {

        instance.fodLevelStart.Raise(instance, null);

    }


}
