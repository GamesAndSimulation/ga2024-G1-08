using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogFinalSequence : MonoBehaviour
{

    public Transform player, finalPosition;
    public Level1DogStateHandler dogStateHandler;
    private const float WAITTIME_MOVETOPLAYER = 4.0f;
    private const float WAITTIME_MOVETOFINALPOSITION = 3.0f;
    private const float NEWTARGETRADIUS = 2.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToPositions()
    {
        dogStateHandler.targetRadius = NEWTARGETRADIUS;
        StartCoroutine(aux());
    }

    IEnumerator aux()
    {
        yield return new WaitForSeconds(WAITTIME_MOVETOPLAYER);

        dogStateHandler.SniffUpTarget(player);
        
        while(dogStateHandler.currentState != Level1DogStateHandler.DogState.sniffingUp)
        {
            yield return null;
        }

        yield return new WaitForSeconds(WAITTIME_MOVETOFINALPOSITION);

        dogStateHandler.StopSniffing();

        while (dogStateHandler.currentState != Level1DogStateHandler.DogState.idle)
        {
            yield return null;
        }

        dogStateHandler.MoveDogToTarget(finalPosition);

    }
}
