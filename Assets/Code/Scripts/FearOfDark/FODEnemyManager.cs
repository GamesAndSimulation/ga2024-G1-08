using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FODEnemyManager : MonoBehaviour
{

    public static FODEnemyManager instance { get; private set; }

    private void Awake() {

        if (instance == null)
            instance = this;

        player = PlayerWatcherComponent.getPlayer();

    }

    [Header("Eye")]
    [SerializeField] private GameObject eyePrefab;
    [SerializeField] private float eyeSpawnInitialDelay = 20;
    [SerializeField] private float eyeSpawnDistance = 200f;
    [SerializeField] private float eyeHeight = 1;
    [SerializeField] private Vector2 eyepawnDelayRange = new Vector2(10, 30);

    [Header("Behind")]
    [SerializeField] private GameObject behindPrefab;
    [SerializeField] private float behindSpawnInitialDelay = 5;
    [SerializeField] private Vector2 behindSpawnDistanceRange = new Vector2(15, 25);
    [SerializeField] private Vector2 behindSpawnDelayRange = new Vector2(10, 30);

    private GameObject player;


    private void Start() {

        Invoke(nameof(spawnBehind), behindSpawnInitialDelay);
        Invoke(nameof(spawnEye), eyeSpawnInitialDelay);

    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void behindWasKilled() {

        Invoke(nameof(spawnBehind), Random.Range(behindSpawnDelayRange[0], behindSpawnDelayRange[1]));


    }

    public void spawnBehind() {

        float distanceBehind = Random.Range(behindSpawnDistanceRange[0], behindSpawnDistanceRange[1]);

        GameObject behind = Instantiate(behindPrefab, transform);

        Vector3 behindVector = player.transform.forward * -1;

        behind.transform.position = player.transform.position + behindVector * distanceBehind;

        Debug.Log("Spawned behind at " + behind.transform.position);

    }

    public void spawnEye() {

        GameObject eye = Instantiate(eyePrefab, transform);

        Vector3 forwardVector = player.transform.forward;

        forwardVector.y = 0;

        eye.transform.position = player.transform.position + forwardVector * eyeSpawnDistance;

        eye.transform.position = new Vector3(eye.transform.position.x, eyeHeight, eye.transform.position.z);

        Debug.Log("Spawned eye at " + eye.transform.position);

    }

}
