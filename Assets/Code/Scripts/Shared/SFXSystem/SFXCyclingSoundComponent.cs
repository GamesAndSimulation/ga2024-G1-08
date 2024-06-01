using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXCyclingSoundComponent : SFXSoundComponent
{

    [SerializeField] private List<AudioClip> clips;

    private int nextClip;

    public new void PlaySound() {

        audioSource.Play();
        nextClip = (nextClip + 1) % clips.Count;
        audioSource.clip = clips[nextClip];

    }

    // Start is called before the first frame update
    void Start()
    {
        nextClip = 0;
        audioSource.clip = clips[0];

    }


}
