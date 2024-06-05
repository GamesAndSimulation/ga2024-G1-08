using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsManager : MonoBehaviour
{

    public static LevelsManager instance { get; private set; }

    [SerializeField] private GameEvent fodLevelStart; 

    private void Awake() {

        if (instance == null)
            instance = this;


        DontDestroyOnLoad(this);

    }

    private int currentLevel = 1;

    public void OnStartLevel1(Component sender, object data) {

        unloadCurrentLevel();
        transitionToFirstLevel();
    
    }

    public void transitionToFirstLevel() {

        SceneManager.LoadScene("Level1", LoadSceneMode.Additive);
        currentLevel = 1;
    }

    public void transitionToSecondLevel() {

        SceneManager.LoadScene("FearOfDarkScene", LoadSceneMode.Additive);
        currentLevel = 2;
    }

    public void unloadFirstLevel() {

        SceneManager.UnloadSceneAsync("Level1");
    }
    public void unloadFODLevel() {

        SceneManager.UnloadSceneAsync("FearOfDarkScene");
    }


    public void unloadCurrentLevel() {

        switch(currentLevel) {

            case 1:
                unloadFirstLevel(); break;

            case 2:
                unloadFODLevel(); break;


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
