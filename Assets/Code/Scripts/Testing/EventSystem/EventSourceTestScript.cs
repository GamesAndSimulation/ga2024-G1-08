using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSourceTestScript : MonoBehaviour
{

    [SerializeField]
    private string RandomString = "Bart";

    [SerializeField]
    private int RandomInt = 7;

    [Header("Events")]

    [SerializeField]
    private GameEvent eventA;

    [SerializeField]
    private GameEvent eventB;


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
            eventA.Raise(this, "carambolas");

        if (Input.GetKeyDown(KeyCode.B))
            eventB.Raise(this, Time.time);


    }
}
