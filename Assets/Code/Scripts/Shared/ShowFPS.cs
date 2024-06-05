using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowFPS : MonoBehaviour
{
    private int fps;
    private bool toggle;
    private float time;
    public TextMeshProUGUI fpsText;

    // Start is called before the first frame update
    void Start()
    {
        toggle = false;
        time = 0;
        fps = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (toggle)
        {
            fpsText.text = fps.ToString();
        }
        else
        {
            fpsText.text = "";
        }

        if (time > 1.0f)
        {
            time = 0;

            fps = Mathf.RoundToInt(1 / Time.deltaTime);

        }
        
    }

    public void ToggleFPS()
    {
        toggle = !toggle;
        
    }
}
