using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FODDogController : MonoBehaviour
{

    private GameObject player;

    private DogStateHandler dogStateHandler;

    private IsVisibleChecker isVisibleChecker;
        
    [SerializeField] private GameObject pointLight;

    [SerializeField] private float targetDistance = 15;

    [SerializeField] private float playerMinimumProximity = 3.5f;




    void Awake() {

        player = PlayerWatcherComponent.getPlayer();
        isVisibleChecker = GetComponentInChildren<IsVisibleChecker>();
        dogStateHandler = GetComponent<DogStateHandler>();
    
    }

    private void Start() {
    }


    // Update is called once per frame
    void Update()
    {
        if(!isVisibleChecker.isVisible() && pointLight.activeSelf == false) {

            pointLight.SetActive(true);
            dogStateHandler.playSingleBark();
            generateTarget();


        } else {

            Vector3 vectorToPlayer = new Vector3(transform.position.x - player.transform.position.x, 0, transform.position.z - player.transform.position.z);

            if (vectorToPlayer.magnitude <= playerMinimumProximity) {
                dogStateHandler.goToTarget();

            } else {
                dogStateHandler.stopMoving();
            }
            //dogAnimation.MovingAnim(0);

        }

    }

    public void generateTarget() {

        Vector3 vectorToPlayer = new Vector3(transform.position.x - player.transform.position.x, 0, transform.position.z - player.transform.position.z);

        Vector3 targetPos = transform.position - vectorToPlayer.normalized * targetDistance;
        GameObject gameObjectTarget = new GameObject("target");
        gameObjectTarget.transform.position = targetPos;
        dogStateHandler.setTarget(gameObjectTarget.transform);

        Debug.Log("Generated target to dog at pos: " + targetPos);


    }
}
