using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{

    public static SFXManager instance { get; private set; }

    [SerializeField] private AudioSource baseAudioSource;

    private void Awake() {
        
        if(instance == null)
            instance = this;

        else
            Destroy(gameObject);


        DontDestroyOnLoad(this);

    }

   public void PlayClip(AudioClip clip, AudioSource source, float delay = 0) {

        source.clip = clip;

        if (delay > 0)
            source.PlayDelayed(delay);

        else
            source.Play();

   }

   public void PlayClip(AudioClip clip, Transform transform, float delay = 0) {

        AudioSource createdSource = Instantiate(baseAudioSource);
        createdSource.transform.position = transform.position;

        PlayClip(clip, createdSource, delay);

        Destroy(createdSource, delay + clip.length + 0.2f); //destroys de source after the audio is played

    }

}
