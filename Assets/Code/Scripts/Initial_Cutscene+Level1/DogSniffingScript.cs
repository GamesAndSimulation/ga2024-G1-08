using System.Collections;
using System.Collections.Generic;
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
    


    // Start is called before the first frame update
    void Start()
    {

        index = 0;

        if (targets.Count > 0)
        {
            timeLimit = level1.totalTime * 60 / targets.Count;
            currentTarget = targets[index];
            StartCoroutine(StartSniffing(0.5f));
           
        }
        else
        {
            timeLimit = level1.totalTime * 60;
        }
    }

    // Update is called once per frame
    void Update()
    {
        int activeChildren = 0;
        
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
            if (index < targets.Count - 1)
            {
                
                index++;
                currentTarget = targets[index];

               if(currentTarget.localPosition.y > heightLimit)
                    dogStateHandler.SniffUpTarget(currentTarget);
                else
                 dogStateHandler.SniffDownTarget(currentTarget);
            }
            else
            {
                dogStateHandler.StopSniffing();
            }
        }

    }

    IEnumerator StartSniffing(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        if (currentTarget.localPosition.y > heightLimit)
            dogStateHandler.SniffUpTarget(currentTarget);
        else
            dogStateHandler.SniffDownTarget(currentTarget);
    }

}
