using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTriggerRaiseEvent : MonoBehaviour
{
  
    [SerializeField]
    private GameEvent gameEvent;

    private void OnTriggerEnter(Collider other)
    {
        gameEvent.Raise(this, other.gameObject.tag);
    }
}
