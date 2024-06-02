using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class SFXSoundComponent : MonoBehaviour
{

    [SerializeField] public AudioSource audioSource;

    public void PlaySound() {

        audioSource.Play();

    }


}
