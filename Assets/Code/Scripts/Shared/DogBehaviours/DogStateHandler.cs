using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//This class is responsible to handle the dog's state machine
public class DogStateHandler : MonoBehaviour
{
    [Header("State")]
    public DogState currentState;

    [Header("State Changers")]
    private bool hasReachedTarget;
    private Transform target;
    public float targetRadius = 0.5f;
    public Transform ballTarget; //this is for debugging purposes

    [Header("Scripts")]
    public DogMovement moveScript;
    public DogAnimation animScript;
    public DogSounds soundScript;

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
                
                hasReachedTarget = Vector3.Distance(transform.position, target.position) < targetRadius;

                if(hasReachedTarget)
                {
                    currentState = DogState.idle;
                    animScript.StartIdle();
                    hasReachedTarget = false;
                }
                else
                {
                    animScript.MovingAnim(moveScript.GetSpeed());
                    moveScript.WalkToTarget(target);
                }
                
                break;
        }
    }

    public void MoveDogToTarget(Transform target)
    {
        this.target = target;
        currentState = DogState.moving;
        hasReachedTarget = false;
    }


    void CheckForOrders()
    {
        if(Input.GetKeyDown("f"))
        {
            target = ballTarget.transform;
            currentState = DogState.moving;
            hasReachedTarget = false;
            
        }

        if (Input.GetKeyDown("g"))
        {
            animScript.PlaySingleBark();
            soundScript.PlaySingleBark();
        }
    }
}
