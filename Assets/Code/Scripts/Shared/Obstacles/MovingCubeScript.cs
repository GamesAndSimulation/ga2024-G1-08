using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingCubeScript : MonoBehaviour
{

    public float baseSpeed = 0.5f;

    public List<GameObject> referenceCubes; //the cubes used as a reference to decide the course of this cube

    private List<Vector3> positions; //the actual positions of the course

    //means that the last and first pos must be the same and the movement does not invert
    public bool continuous = false;

    private Vector3 currentPrevPos;
    private Vector3 currentNextPos;
    private Vector3 dirVectorFromPos;
    private float currentPosDis;
    private float distanceMovedSinceLastPos;

    private int prevPosIndex;
    private bool indexIncreasing;


    // Start is called before the first frame update
    void Start()
    {

        positions = new List<Vector3>(referenceCubes.Count);

        for (int i = 0; i < referenceCubes.Count; i++) {

            positions.Add(new Vector3(referenceCubes[i].transform.position.x, referenceCubes[i].transform.position.y, referenceCubes[i].transform.position.z));
            Destroy(referenceCubes[i]);
        }

        currentPrevPos = positions[0];
        currentNextPos = positions[1];
        currentPosDis = Vector3.Distance(currentPrevPos, currentNextPos);
        distanceMovedSinceLastPos = 0;
        dirVectorFromPos = (currentNextPos - currentPrevPos).normalized;

        prevPosIndex = 0;
        indexIncreasing = true;



    }

    // Update is called once per frame
    void Update()
    {

        //if we moved enough
        if (distanceMovedSinceLastPos >= currentPosDis) 
            updatePos();


        float toMoveInThisFrame = baseSpeed * Time.deltaTime; //declaring this here is bad performance?

        transform.Translate(toMoveInThisFrame * dirVectorFromPos, Space.World);

        distanceMovedSinceLastPos += toMoveInThisFrame;



    }

    /**
     * Updates everything regarding the positions, the indexes and the distance
     */
    private void updatePos() {

        if (continuous)
            updatePosIndexContMov();

        else
            updatePosIndexInvMov();

        currentPosDis = Vector3.Distance(currentPrevPos, currentNextPos);

    }

    /**
     * Updates position index and position tracking having in account continous movement
     */
    private void updatePosIndexContMov() {

        currentPrevPos = positions[(prevPosIndex + 1) % positions.Count];
        currentNextPos = positions[(prevPosIndex + 2) % positions.Count];

        prevPosIndex = (prevPosIndex + 1) % positions.Count;

    }

    /**
     *  Updates position index and position tracking having in account invertable movement
     */
    private void updatePosIndexInvMov() {

        //if we were increasing the index
        if (indexIncreasing) {

            //and we reached the final position, we invert the direction of movement
            if (prevPosIndex >= positions.Count - 2) {

                indexIncreasing = false; //we'll now go backwards
                currentPrevPos = positions[prevPosIndex + 1]; //the last pos we visited was the last of the movementm
                currentNextPos = positions[prevPosIndex]; //the next pos we'll visit was the previous pos
                prevPosIndex = prevPosIndex + 1;


            } else { //else, we continue to advance in our movement

                currentPrevPos = positions[prevPosIndex + 1];
                currentNextPos = positions[prevPosIndex + 2];
                prevPosIndex = prevPosIndex + 1;


            }

            //if we were decreasing the index
        } else {

            //and we reached the final position, we invert the direction of movement
            if (prevPosIndex <= 1) {

                indexIncreasing = true; //we'll now go forward
                currentPrevPos = positions[prevPosIndex - 1]; //the last pos we visited was the last of the movement
                currentNextPos = positions[prevPosIndex]; //the next pos we'll visit was the previous pos
                prevPosIndex = prevPosIndex - 1;


            } else { //else, we continue to advance in our movement

                currentPrevPos = positions[prevPosIndex - 1];
                currentNextPos = positions[prevPosIndex - 2];
                prevPosIndex = prevPosIndex - 1;


            }

        }

        distanceMovedSinceLastPos = 0;
        dirVectorFromPos = (currentNextPos - currentPrevPos).normalized;

    }

}
