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

    private int currentLevel = 1;

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

    private IEnumerator setActiveScene(string sceneName) {

        yield return null; // Wait one frame for the scene to load

        Scene newScene = SceneManager.GetSceneByName(sceneName);

        if (newScene.isLoaded) {
            // Set active scene to apply its lighting settings
            SceneManager.SetActiveScene(newScene);
            canTransitionScene = true;
        }
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

    public void unloadFirstLevel() {

    }
    public void unloadFODLevel() {

        
    }


    public void unloadCurrentLevel() {

        switch(currentLevel) {

            case 1:
                SceneManager.UnloadScene("Level1");

                break;

            case 2:
                SceneManager.UnloadScene("FearOfDarkScene"); 
                break;


        }

    }


    public void transitionToFOD() {

        instance.fodLevelStart.Raise(instance, null);

    }

    // Start is called before the first frame update
    void Start()
    {

        transitionToFirstLevel();


    }

}
