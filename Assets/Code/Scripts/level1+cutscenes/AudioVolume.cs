using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVolume : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator ReduceVolume(AudioSource audio, float audioFadeTime)
    {
        float startVolume = audio.volume;

        while (audio.volume > 0)
        {
            audio.volume -= startVolume * Time.deltaTime / audioFadeTime;

            yield return null;
        }

        audio.Stop();
        audio.volume = startVolume; 

    }

    public IEnumerator IncreaseVolume(AudioSource audio, float audioFadeTime)
    {
        audio.Play();
        float startVolume = audio.volume;
        audio.volume = 0;

        while (audio.volume < startVolume)
        {
            audio.volume += startVolume * Time.deltaTime / audioFadeTime;
            yield return null;

        }
        
    }
}
