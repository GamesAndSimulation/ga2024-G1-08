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


    public void transitionToFirstLevel() {

        SceneManager.LoadScene("Level1", LoadSceneMode.Additive);

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
