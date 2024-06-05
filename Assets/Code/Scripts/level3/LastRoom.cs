using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastRoom : MonoBehaviour
{

    public GameEvent gameEvent;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameEvent.Raise(this, null);
        }
    }

}
