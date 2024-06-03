using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectAsButton : MonoBehaviour
{

    public GameEvent gameEvent;

    private void OnMouseUpAsButton()
    {
        gameEvent.Raise(this, "");
    }
}
