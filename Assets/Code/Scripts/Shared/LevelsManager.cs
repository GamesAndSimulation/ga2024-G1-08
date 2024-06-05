using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsManager : MonoBehaviour
{

    public static LevelsManager instance { get; private set; }

    [Header("Events")]

    [SerializeField] private GameEvent fodLevelStart;


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
        transitionToInitialScene();
    }


    public void transitionToLevel1() {
        transitionToFirstLevel();

    }

    public void transitionToLevel2() {
         transitionToSecondLevel();
    }

    public void transitionToBadEndingCutscene()
    {
        transitionToBadEnding();
    }

    public void transitionToGoodEndingCutscene()
    {
        transitionToGoodEnding();
    }

  

    private void transitionToInitialScene()
    {
        SceneManager.LoadScene("InitialCutscene", LoadSceneMode.Single);
        currentLevel = 0;
    }

    private void transitionToFirstLevel() {

        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
        currentLevel = 1;
    }

    private void transitionToSecondLevel() {

        SceneManager.LoadScene("FearOfDarkScene", LoadSceneMode.Single);
        currentLevel = 2;
    }

    private void transitionToBadEnding()
    {

        SceneManager.LoadScene("BadEnding", LoadSceneMode.Single);
        currentLevel = 5;
    }

    private void transitionToGoodEnding()
    {

        SceneManager.LoadScene("GoodEnding", LoadSceneMode.Additive);
        currentLevel = 6;
    }


    public void transitionToFOD() {

        instance.fodLevelStart.Raise(instance, null);

    }


}
