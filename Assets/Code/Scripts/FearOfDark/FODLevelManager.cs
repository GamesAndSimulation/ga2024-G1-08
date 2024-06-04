using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class FODLevelManager : MonoBehaviour
{

    public static FODLevelManager instance { get; private set; }

    private void Awake() {

        if (instance == null)
            instance = this;

        PlayerWatcherComponent.addSubToPlayerChanged(changePlayer);


    }

    [SerializeField] private GameObject fodPlayerAndCam;
    private GameObject player;


    [SerializeField] private float spawnDogInitialDelay = 10;
    [SerializeField] private float spawnDogInitialDistance = 5;
    [SerializeField] private float spawnYValue = 0.5f;

    [SerializeField] private GameObject dogPrefab;

    public void changePlayer(GameObject newPlayer) {

        Debug.Log("Changed player to " + newPlayer);

        player = newPlayer;

    }

    // Start is called before the first frame update
    void Start()
    {

        player = PlayerWatcherComponent.getPlayer();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnFODLevelStart(Component sender, object data) {

        fodPlayerAndCam.SetActive(true);
        Invoke(nameof(SpawnDog), spawnDogInitialDelay);

    }

    public void SpawnDog() {

        Debug.Log(player);

        Vector3 playerPosition = player.transform.position - transform.position;

        Vector3 playerForward = player.transform.forward;

        int randomRotation = Random.Range(0, 360);

        Vector3 vectorToDog = Quaternion.Euler(0, randomRotation, 0) * playerForward;

        GameObject dog = Instantiate(dogPrefab, this.transform);

        dog.transform.position = playerPosition + vectorToDog * spawnDogInitialDistance;

        dog.transform.position = new Vector3(dog.transform.position.x, spawnYValue, dog.transform.position.z);

        dog.transform.Rotate(new Vector3(0, randomRotation, 0));

        Debug.Log("Spawned dog at " + dog.transform.position);

        dog.GetComponent<DogSounds>().PlaySingleBark();


    }


}
