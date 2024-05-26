using System;
using System.Collections.Generic;
using UnityEngine;

public class DogSounds : MonoBehaviour
{

    public AudioSource audioSource;
    public List<AudioClip> barkClips;

    public int lastBark;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySingleBark()
    {
        int thisBark = lastBark;

        while(thisBark == lastBark)
            thisBark = UnityEngine.Random.Range(0, barkClips.Count);

        audioSource.clip = barkClips[thisBark];
        lastBark = thisBark;
        audioSource.Play();
    }
}
