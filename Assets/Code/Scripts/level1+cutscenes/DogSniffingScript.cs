using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DogSniffingScript : MonoBehaviour
{

    public List<Transform> targets;
    public Level1DogStateHandler dogStateHandler;
    public Level1MainScript level1;
    public float heightLimit = 0.45f;

    private Transform currentTarget;
    private int index;
    private float time = 0;
    private float timeLimit;
    private bool isDogMoving = true;

    private const float WAITTIME_STARTSNIFFING = 0.5f;


    // Start is called before the first frame update
    void Start()
    {

        index = 0;

        if (targets.Count > 0)
        {
            timeLimit = level1.totalTime * 60 / targets.Count;
            currentTarget = targets[index];
            StartCoroutine(StartSniffing());
           
        }
        else
        {
            timeLimit = level1.totalTime * 60;
        }
   
    }

    // Update is called once per frame
    void Update()
    {
        if (isDogMoving && !PausedWatcherComponent.paused)
        {
            int activeChildren = 0;
            int aux = 0;

            foreach (Transform child in currentTarget)
            {

                if (child.gameObject.activeInHierarchy)
                {
                    activeChildren++;
                    time += Time.deltaTime;
                    break;
                }
            }

            if (activeChildren == 0 || time > timeLimit)
            {
                time = 0;
                activeChildren = 0;
                index = (index + 1) % targets.Count;
                currentTarget = targets[index];


                while (activeChildren == 0 && aux < targets.Count)
                {
                    foreach (Transform child in currentTarget)
                    {

                        if (child.gameObject.activeInHierarchy)
                        {
                            activeChildren++;
                            time += Time.deltaTime;
                            break;
                        }
                    }

                    if (activeChildren <= 0)
                    {
                        index = (index + 1) % targets.Count;
                        aux++;
                        currentTarget = targets[index];
                    }

                }

                if (aux < targets.Count)
                {
                    if (currentTarget.localPosition.y > heightLimit)
                        dogStateHandler.SniffUpTarget(currentTarget);
                    else
                        dogStateHandler.SniffDownTarget(currentTarget);
                }
                else
                {
                    StopMoving();
                }

            }
        }

    }

    IEnumerator StartSniffing()
    {
        yield return new WaitForSeconds(WAITTIME_STARTSNIFFING);

        if (currentTarget.localPosition.y > heightLimit)
            dogStateHandler.SniffUpTarget(currentTarget);
        else
            dogStateHandler.SniffDownTarget(currentTarget);
    }

    public void StopMoving()
    {
        dogStateHandler.StopMoving();
        dogStateHandler.StopSniffing();
        isDogMoving = false;
    }

}
