using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsManager : MonoBehaviour
{

    public static LevelsManager instance { get; private set; }

    [SerializeField] private int StartingLevel = -1;

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
        switch (StartingLevel) {

            case 0:
                transitionToInitialCutscene();
                break;
            case 1:
                transitionToLevel1();
                break;
            case 2:
                transitionToLevel2();
                break;
            case 3:
                transitionToLevel3();
                break;
            case 4:
                transitionToBadEndingCutscene();
                break;

            case 5:
                transitionToGoodEndingCutscene();
                break;

            default:
                break;
        }

    }

    public void transitionToInitialCutscene()
    {
        SceneManager.LoadScene("InitialCutscene", LoadSceneMode.Single);
    }


    public void transitionToLevel1() {
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);

    }

    public void transitionToLevel2() {
        SceneManager.LoadScene("FearOfDarkScene", LoadSceneMode.Single);
    }

    public void transitionToLevel3()
    {
        SceneManager.LoadScene("MazeSystemScene", LoadSceneMode.Single);
    }

    public void transitionToBadEndingCutscene()
    {
        SceneManager.LoadScene("BadEnding", LoadSceneMode.Single);
    }

    public void transitionToGoodEndingCutscene()
    {
        SceneManager.LoadScene("GoodEnding", LoadSceneMode.Single);
    }

}
