using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public struct Captions
{
    public float time;
    public string text;
}


public class CaptionsScript : MonoBehaviour
{
    public Captions[] captions;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayCaptions()
    {
        StartCoroutine(a());
    }

    IEnumerator a()
    {
        foreach (var caption in captions)
        {
            gameObject.GetComponent<TextMeshProUGUI>().text = caption.text;
            yield return new WaitForSeconds(caption.time);
        }
    }
}
