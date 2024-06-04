using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : MonoBehaviour
{

    public static LevelsManager instance { get; private set; }

    [SerializeField] private GameEvent fodLevelStart; 

    private void Awake() {

        if (instance == null)
            instance = this;


        DontDestroyOnLoad(this);

    }

    public static void transitionToFOD() {

        instance.fodLevelStart.Raise(instance, null);

    }

    // Start is called before the first frame update
    void Start()
    {

        transitionToFOD();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
