using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is responsible to handle the dog's state machine
public class DogStateHandler : MonoBehaviour
{
    [Header("State")]
    public DogState currentState;

    [Header("State Changers")]
    private bool hasReachedTarget;
    private Transform target;
    public Transform ballTarget; //this is for debugging purposes

    [Header("Scripts")]
    public DogMovement moveScript;

    public enum DogState
    {
        idle,
        moving
    }

    void Start()
    {
        currentState = DogState.idle;
        hasReachedTarget = false;
    }

    void Update()
    {
        StateHandler();
        CheckForOrders();
    }

    void StateHandler()
    {
        switch (currentState)
        {
            case DogState.idle:
                //Do nothing
                break;

            case DogState.moving:
                
                hasReachedTarget = (transform.position == target.position);

                if(hasReachedTarget)
                {
                    currentState = DogState.idle;
                    hasReachedTarget = false;
                }
                else
                {
                    moveScript.MoveToTarget(target);
                }
                
                break;
        }
    }

    void CheckForOrders()
    {
        if(Input.GetKeyDown("f"))
        {
            target = ballTarget.transform;
            currentState = DogState.moving;
            hasReachedTarget = false;
            Debug.Log("Moving to target " + transform.position);
            
        }   
    }
}
