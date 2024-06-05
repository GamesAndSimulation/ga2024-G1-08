using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent")] //will allow to create this in the scene
public class GameEvent : ScriptableObject //to store data shared among objects
{
    [TextArea(1, 10)]
    public string description;

    public List<GameEventListener> listeners = new List<GameEventListener>();
   
    public void Raise(Component sender, object data) {

        Debug.Log(sender.gameObject.name + " has raised " + name + " event with " + listeners.Count + " listeners");

        foreach(GameEventListener listener in listeners)
            listener.OnEventRaised(sender, data);

    }

    public void RegisterListener(GameEventListener listener) {
    
        if(!listeners.Contains(listener))
            listeners.Add(listener);
        
    }

    public void UnregisterListener(GameEventListener listener) {

        if(listeners.Contains(listener))
            listeners.Remove(listener);

    }

}
