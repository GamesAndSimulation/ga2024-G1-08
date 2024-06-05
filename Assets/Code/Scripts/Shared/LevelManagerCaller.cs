using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerCaller : MonoBehaviour
{
   
    public void transitionToInitialCutscene()
    {
        LevelsManager.instance.transitionToInitialCutscene();
    }
}
