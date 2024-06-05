using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsManager : MonoBehaviour
{

    public static LevelsManager instance { get; private set; }

    [Header("Events")]
    [SerializeField] private GameEvent firstLevelStart;
    [SerializeField] private GameEvent firstLevelEnd;

    [SerializeField] private GameEvent fodLevelStart;
    [SerializeField] private GameEvent fodLevelEnd;
    


    private void Awake() {

        if (instance == null)
            instance = this;

        else
            Destroy(gameObject);


        DontDestroyOnLoad(this);

    }

    private int currentLevel = 1;

    public void OnStartLevel1(Component sender, object data) {

        unloadCurrentLevel();
        transitionToFirstLevel();
    
    }

    public void OnStartLevel2(Component sender, object data) {

        unloadCurrentLevel();
        transitionToSecondLevel();

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

    }
    public void unloadFODLevel() {

        
    }


    public void unloadCurrentLevel() {

        switch(currentLevel) {

            case 1:
                firstLevelEnd.Raise(this, null);
                SceneManager.UnloadSceneAsync("Level1");

                break;

            case 2:
                fodLevelEnd.Raise(this, null);
                SceneManager.UnloadSceneAsync("FearOfDarkScene"); 
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
