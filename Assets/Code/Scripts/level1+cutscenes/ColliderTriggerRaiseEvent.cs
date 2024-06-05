using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTriggerRaiseEvent : MonoBehaviour
{
  
    [SerializeField]
    private GameEvent gameEvent;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        gameEvent.Raise(this, other.gameObject.tag);
    }
}
