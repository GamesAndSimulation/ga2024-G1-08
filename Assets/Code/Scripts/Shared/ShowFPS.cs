using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowFPS : MonoBehaviour
{
    private int fps;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fps = Mathf.RoundToInt(1/Time.deltaTime);
    }

    public void ToggleFPS()
    {

    }
}
