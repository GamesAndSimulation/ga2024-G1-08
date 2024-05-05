using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventListenerTestScript : MonoBehaviour
{

    [SerializeField]
    private string RandomString = "Homer";

    [SerializeField]
    private int RandomInt = 13;



    public void onEventAOrB(Component sender, object data) {

        if(data is string)
            Debug.Log("Event listener " + gameObject.name + " received an event with a string: " + (string)data);

        else if (data is float)
            Debug.Log("Event listener " + gameObject.name + " received an event with an int: " + (float)data);

    }
    

}
