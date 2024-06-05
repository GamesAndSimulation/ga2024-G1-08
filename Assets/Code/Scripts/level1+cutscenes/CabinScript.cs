using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinScript : MonoBehaviour
{
    public GameObject triggerPoint;
    public GameObject darkRoom;
    public AudioSource wallsClosing;
    public string PLAYERTAG = "Player";

  

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnPlayerEnteredCabin(Component sender, object data) {

        if((string)data == PLAYERTAG)
        {
            darkRoom.SetActive(true);
            triggerPoint.SetActive(false);
            wallsClosing.Play();
        }
    }
}
