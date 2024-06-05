using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//This class is responsible to handle the dog's state machine
public class Level1DogStateHandler : MonoBehaviour
{
    [Header("State")]
    public DogState currentState;

    [Header("State Changers")]
    private bool hasReachedTarget;
    private Transform target;
    public float targetRadius = 1.4f;

    [Header("Scripts")]
    public DogMovement moveScript;
    public DogAnimation animScript;
    public DogSounds soundScript;

    private bool sniffingUp;
    private bool sniffingDown;

    public enum DogState
    {
        idle,
        moving,
        sniffingUp,
        sniffingDown,
        stopSniffingUp,
        stopSniffingDown
    }

    void Start()
    {
        currentState = DogState.idle;
        hasReachedTarget = false;
        sniffingUp = false;
        sniffingDown = false;
        target = null;
    }

    void Update()
    {
        StateHandler();
    }

    void StateHandler()
    {
        switch (currentState)
        {

            case DogState.idle:
                {
                    //Do nothing
                    break;
                }
            case DogState.sniffingUp:
                {
                    //Do nothing
                    break;
                }
            case DogState.sniffingDown:
                {
                    //Do nothing
                    break;
                }

            case DogState.stopSniffingUp:
                {
                    animScript.StopSniffUp();
                    currentState = DogState.idle;
                    sniffingUp = false;
                    break;
                }
            case DogState.stopSniffingDown:
                {
                    animScript.StopSniffDown();
                    currentState = DogState.idle;
                    sniffingDown = false;
                    break;
                }
            case DogState.moving:
                {
                    hasReachedTarget = Vector3.Distance(transform.position, target.position) < targetRadius;

                    if (hasReachedTarget)
                    {
                        moveScript.StopWalking();
                        currentState = DogState.idle;
                        animScript.StartIdle();


                        if (sniffingDown)
                        {
                            animScript.SniffDown();
                            currentState = DogState.sniffingDown;

                        }
                        else if (sniffingUp)
                        {
                            animScript.SniffUp();
                            currentState = DogState.sniffingUp;
                        }

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
    }

    public void StartSniffingUp()
    {
        if (currentState == DogState.idle)
        {
            animScript.SniffUp();
            currentState = DogState.sniffingUp;
        }
    }

    public void MoveDogToTarget(Transform target)
    {
        this.target = target;
        currentState = DogState.moving;
        hasReachedTarget = false;
    }

    public void SniffDownTarget(Transform target)
    {
        if (this.target != target)
        {
            StopSniffing();
            StartCoroutine(Wait(target));
        }
    }

    public void SniffUpTarget(Transform target)
    {
        if (this.target != target)
        {
            StopSniffing();
            StartCoroutine(Wait2(target));
        }
    }

    public void StopSniffing()
    {
        if (sniffingUp)
        {
            currentState = DogState.stopSniffingUp;
        }
        else if (sniffingDown)
        {
            currentState = DogState.stopSniffingDown;
        }
    }

    public void StopMoving()
    {
        if (currentState == DogState.moving)
        {
            moveScript.StopWalking();
            currentState = DogState.idle;
            animScript.StartIdle();
            sniffingDown = false;
            sniffingUp = false;
        }
    }


    IEnumerator Wait(Transform target)
    {
        yield return new WaitForSeconds(3.0f);
        sniffingDown = true;
        MoveDogToTarget(target);

    }


    IEnumerator Wait2(Transform target)
    {
        yield return new WaitForSeconds(3.0f);
        sniffingUp = true;
        MoveDogToTarget(target);

    }
}
