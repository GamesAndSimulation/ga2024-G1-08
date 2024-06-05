using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinScript : MonoBehaviour
{
    public GameObject triggerPoint;
    public GameObject darkRoom;
    public AudioSource wallsClosing;
    public string PLAYERTAG = "Player";


    public Light lightInScene; //the light that is illuminating the whole scene
    public Light newLight; //a new light to use when all becomes dark


    public void OnPlayerEnteredCabin(Component sender, object data) {

        if((string)data == PLAYERTAG)
        {
            darkRoom.SetActive(true);
            triggerPoint.SetActive(false);
            wallsClosing.Play();
            lightInScene.enabled = false;
            newLight.enabled = true;
            RenderSettings.ambientIntensity = 0;
            RenderSettings.reflectionIntensity = 0;
        }
    }

}
