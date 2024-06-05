using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FODDogController : MonoBehaviour
{

    private GameObject player;

    private FODDogStateHandler dogStateHandler;

    private IsVisibleChecker isVisibleChecker;
        
    [SerializeField] private GameObject pointLight;

    [SerializeField] private float targetDistance = 15;

    [SerializeField] private float playerMinimumProximity = 3.5f;

    [Header("Events")]
    [SerializeField] private GameEvent fodDogReachedTarget;


    void Awake() {

        player = PlayerWatcherComponent.getPlayer();
        isVisibleChecker = GetComponentInChildren<IsVisibleChecker>();
        dogStateHandler = GetComponent<FODDogStateHandler>();
    
    }

    private void Start() {
    }


    // Update is called once per frame
    void Update()
    {
        if(!isVisibleChecker.isVisible()) {

            if (dogStateHandler.hasReachedTarget) {

                fodDogReachedTarget.Raise(this, null);
                Destroy(gameObject);


            }   
            else if(pointLight.activeSelf == false) {

                pointLight.SetActive(true);
                dogStateHandler.playSingleBark();
                generateTarget();


            }



        } else {

            Vector3 vectorToPlayer = new Vector3(transform.position.x - player.transform.position.x, 0, transform.position.z - player.transform.position.z);

            if (vectorToPlayer.magnitude <= playerMinimumProximity) {

                dogStateHandler.goToTarget();

            } else {
                dogStateHandler.stopMoving();
            }

        }

    }

    public void generateTarget() {


        Vector3 vectorToPlayer = new Vector3(transform.position.x - player.transform.position.x, 0, transform.position.z - player.transform.position.z);

        //generate targetPos in opposite direction
        Vector3 vectorToTarget = -vectorToPlayer.normalized;

        //calculate position of target
        Vector3 targetPos = transform.position + vectorToTarget * targetDistance;
        
        //set target
        GameObject gameObjectTarget = new GameObject("target");
        gameObjectTarget.transform.position = targetPos;
        dogStateHandler.setTarget(gameObjectTarget.transform);

        //rotate dog to target
        transform.forward = vectorToTarget;

        Debug.Log("Generated target to dog at pos: " + targetPos);


    }
}
