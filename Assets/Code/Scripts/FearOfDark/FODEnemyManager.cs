using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FODEnemyManager : MonoBehaviour
{

    public static FODEnemyManager instance { get; private set; }

    private void Awake() {

        if (instance == null)
            instance = this;

        currentTimeToSpawnBehind = new List<float>();
        currentTimeToSpawnEye = new List<float>();

    }

    [Header("Eye")]
    [SerializeField] private GameObject eyePrefab;
    [SerializeField] private float eyeSpawnInitialDelay = 20;
    [SerializeField] private float eyeSpawnDistance = 200f;
    [SerializeField] private float eyeHeight = 1;
    [SerializeField] private Vector2 eyepawnDelayRange = new Vector2(10, 30);
    [SerializeField] private float eyeScoreMultiplier;
    private List<float> currentTimeToSpawnEye;

    [Header("Behind")]
    [SerializeField] private GameObject behindPrefab;
    [SerializeField] private float behindSpawnInitialDelay = 5;
    [SerializeField] private Vector2 behindSpawnDistanceRange = new Vector2(15, 25);
    [SerializeField] private Vector2 behindSpawnDelayRange = new Vector2(10, 30);
    [SerializeField] private float behindScoreMultiplier;
    private List<float> currentTimeToSpawnBehind;

    [SerializeField] private GameObject player;

    private void Start() {

        spawnEyeDelayed(eyeSpawnInitialDelay);
        spawnBehindDelayed(behindSpawnInitialDelay);

    }


    // Update is called once per frame
    void Update()
    {
        checkTimers(); ;

    }

    private void checkTimers() {

        if(currentTimeToSpawnEye.Count > 0 && currentTimeToSpawnEye[0] <= 0)
            spawnEye();

        if(currentTimeToSpawnBehind.Count > 0 && currentTimeToSpawnBehind[0] <= 0)
            spawnBehind();

        if(currentTimeToSpawnBehind.Count > 0)
            currentTimeToSpawnBehind[0] -= Time.deltaTime;
        
        if(currentTimeToSpawnEye.Count > 0)
            currentTimeToSpawnEye[0] -= Time.deltaTime;

    }

    public void eyeWasKilled() {

        spawnEyeDelayed();

    }

    public void behindWasKilled() {

        spawnBehindDelayed();

    }

    public void OnFODDogReachedGoal(Component sender, object data) {

        spawnBehindDelayed();
        spawnEyeDelayed();

    }

    public void spawnBehindDelayed() {

        float newDelay = (Random.Range(behindSpawnDelayRange[0], behindSpawnDelayRange[1]));

        if (FODLevelManager.instance.score > 0)
            newDelay = newDelay / (FODLevelManager.instance.score * behindScoreMultiplier);

        spawnBehindDelayed(newDelay);

    }

    public void spawnBehindDelayed(float delay) {

        currentTimeToSpawnBehind.Add(delay);

    }

    public void spawnBehind() {


        currentTimeToSpawnBehind.RemoveAt(0);

        float distanceBehind = Random.Range(behindSpawnDistanceRange[0], behindSpawnDistanceRange[1]);

        GameObject behind = Instantiate(behindPrefab, transform);

        Vector3 behindVector = player.transform.forward * -1;

        behind.transform.position = player.transform.position + behindVector * distanceBehind;

        Debug.Log("Spawned behind at " + behind.transform.position);

    }

    public void spawnEyeDelayed() {

        float newDelay = Random.Range(eyepawnDelayRange[0], eyepawnDelayRange[1]);

        if (FODLevelManager.instance.score > 0)
            newDelay = newDelay / (FODLevelManager.instance.score * eyeScoreMultiplier);

        spawnEyeDelayed(newDelay);

    }

    public void spawnEyeDelayed(float delay) {

        currentTimeToSpawnEye.Add(delay);

    }

    public void spawnEye() {

        currentTimeToSpawnEye.RemoveAt(0);

        GameObject eye = Instantiate(eyePrefab, transform);

        Vector3 forwardVector = player.transform.forward;

        forwardVector.y = 0;

        eye.transform.position = player.transform.position + forwardVector * eyeSpawnDistance;

        eye.transform.position = new Vector3(eye.transform.position.x, eyeHeight, eye.transform.position.z);

        Debug.Log("Spawned eye at " + eye.transform.position);

    }

}
