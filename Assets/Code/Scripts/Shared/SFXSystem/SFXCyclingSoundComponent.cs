using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXCyclingSoundComponent : SFXSoundComponent
{

    [SerializeField] private List<AudioClip> clips;

    private int prevClip;

    public new void PlaySound() {

        prevClip = (prevClip + 1) % clips.Count;
        audioSource.clip = clips[prevClip];
        audioSource.Play();

    }

    // Start is called before the first frame update
    void Start()
    {
        prevClip = -1;
        audioSource.clip = clips[0];

    }
}
