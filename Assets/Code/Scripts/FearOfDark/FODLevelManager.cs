using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

public class FODLevelManager : MonoBehaviour
{

    public static FODLevelManager instance { get; private set; }

    private void Awake() {

        if (instance == null)
            instance = this;

    }


    [SerializeField] private float spawnDogInitialDelay = 10;
    [SerializeField] private Vector2 spawnDogDelayRange = new Vector2(20, 30);
    private float currentDelayToSpawnDog;
    private bool toSpawnDog;
    [SerializeField] private float spawnDogInitialDistance = 5;
    [SerializeField] private float spawnYValue = 0.5f;

    [SerializeField] private GameObject dogPrefab;

    [SerializeField] public int scoreToWin = 3;
    public int score;

    [SerializeField] private GameObject player;

    [SerializeField] private AnimationClip turnOffPlayerAnimation;

    [SerializeField] private GameEvent turnOffPlayer;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        spawnDogDelayed(spawnDogInitialDelay);


    }

    // Update is called once per frame
    void Update()
    {

        if (!PausedWatcherComponent.paused) {

            if (currentDelayToSpawnDog <= 0 && toSpawnDog)
                SpawnDog();

            if (toSpawnDog)
                currentDelayToSpawnDog -= Time.deltaTime;

        }
    }

    public  void OnFODDogReachedTarget(Component sender, object data) {

        score += 1;

        if (score >= scoreToWin)
            StartCoroutine(winAfterAnimation());

        else
            spawnDogDelayed(Random.Range(spawnDogDelayRange[0], spawnDogDelayRange[1]));

    }

    public IEnumerator winAfterAnimation() {

        turnOffPlayer.Raise(this, null);
        yield return new WaitForSeconds(turnOffPlayerAnimation.length + 0.01f);
        LevelsManager.instance.transitionToLevel3();

    }

    public void spawnDogDelayed(float delay) {
        currentDelayToSpawnDog = delay;
        toSpawnDog = true;

    }

    public void SpawnDog() {

        toSpawnDog = false;

        Vector3 playerPosition = player.transform.position;

        Vector3 playerForward = player.transform.forward;

        int randomRotation = Random.Range(0, 360);

        Vector3 vectorToDog = Quaternion.Euler(0, randomRotation, 0) * playerForward;

        GameObject dog = Instantiate(dogPrefab, this.transform);

        dog.transform.position = playerPosition + vectorToDog * spawnDogInitialDistance;

        dog.transform.position = new Vector3(dog.transform.position.x, spawnYValue, dog.transform.position.z);

        dog.transform.Rotate(new Vector3(0, randomRotation, 0));

        Debug.Log("Spawned dog at " + dog.transform.position);


    }


}
