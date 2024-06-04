using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FODDogController : MonoBehaviour
{

    private GameObject player;

    private DogSounds dogSounds;

    private DogAnimation dogAnimation;

    private IsVisibleChecker isVisibleChecker;
    
    private Vector3 target;
    
    [SerializeField] private GameObject pointLight;

    [SerializeField] private float targetDistance = 15;

    [SerializeField] private float playerMinimumProximity = 3.5f;




    void Awake() {
        player = PlayerWatcherComponent.getPlayer();
        isVisibleChecker = GetComponentInChildren<IsVisibleChecker>();
        dogSounds = GetComponent<DogSounds>();
        dogAnimation = GetComponent<DogAnimation>();
    }

    private void Start() {
        

    }


    // Update is called once per frame
    void Update()
    {
        if(!isVisibleChecker.isVisible() && pointLight.activeSelf == false) {

            pointLight.SetActive(true);
            dogSounds.PlaySingleBark();


        } else {

            Vector3 vectorToPlayer = new Vector3(transform.position.x - player.transform.position.x, 0, transform.position.z - player.transform.position.z);

            if (vectorToPlayer.magnitude <= playerMinimumProximity) {


            }

        }



    }

    public void generateTarget(Vector3 vectorToPlayer) {

        target = transform.position - vectorToPlayer.normalized * targetDistance;


    }
}
