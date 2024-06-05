using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EyeSpawner : MonoBehaviour
{

    [SerializeField] private int maxEyes = 20;
    public List<GameObject> eyes;

    private GameObject player;

    public GameObject eyePrefab;

    [SerializeField] private float minimumDistance = 0.1f;
    [SerializeField] private float distancePerEye = 0.05f;

    [SerializeField] private float initialDelayToSpawnEye = 2;
    [SerializeField] private float delayReductionMultiplier = 0.5f;
    [SerializeField] private float minDelayToSpawnEye = 0.01f;

    private float currentDelayToSpawnEye;

    [SerializeField] private float initialDelayToKillThis = 10;
    [SerializeField] private float delayToKillThis = 2;

    private float currentDelayToKillThis;

    [SerializeField] private GameEvent damagePlayerEvent;
    [SerializeField] private float damagePerEye = 0.001f;

    private void Awake() {
        player = PlayerWatcherComponent.getPlayer();
    }


    // Start is called before the first frame update
    void Start()
    {

        spawnEye();
        resetDelayToSpawnEye();
        currentDelayToKillThis = initialDelayToKillThis;

    }

    // Update is called once per frame
    void Update()
    {

        currentDelayToKillThis -= Time.deltaTime;
        currentDelayToSpawnEye -= Time.deltaTime;

        if(isVisible()) {

            if (currentDelayToSpawnEye <= 0) {

                if(eyes.Count < maxEyes)
                    spawnEye();
                
                damagePlayerEvent.Raise(this, damagePerEye * eyes.Count);
                resetDelayToSpawnEye();
            }

            resetDelayToKillThis();

        } else {

            if (currentDelayToKillThis <= 0) {
                FODEnemyManager.instance.eyeWasKilled();
                Destroy(gameObject);

            } else
                resetDelayToSpawnEye();

        }
        
    }

    private bool isVisible() {

        bool isVisible = false;

        int eyeIndex = 0;

        while(!isVisible && eyeIndex < eyes.Count) {
            isVisible = eyes[eyeIndex].GetComponentInChildren<IsVisibleChecker>().isVisible();
            eyeIndex++;

        }

        return isVisible;

    }

    private void resetDelayToSpawnEye() {

        currentDelayToSpawnEye = Mathf.Max(initialDelayToSpawnEye / (1 + delayReductionMultiplier * eyes.Count), minDelayToSpawnEye);

    }

    private void resetDelayToKillThis() {

        currentDelayToKillThis = delayToKillThis;

    }

    private void spawnEye() {

        Vector3 betweenEyeAndPlayer = new Vector3( player.transform.position.x - transform.position.x, 0, player.transform.position.z - transform.position.z);

        Vector3 perpendicular = Vector3.Cross(betweenEyeAndPlayer, transform.up).normalized;

        int randomRotation = Random.Range(0, 180);

        Vector3 vectorToEye = Quaternion.Euler(0, 0, randomRotation) * perpendicular;

        GameObject newEye = Instantiate(eyePrefab, this.transform);

        newEye.transform.position = this.transform.position + vectorToEye * (distancePerEye * eyes.Count + minimumDistance);

        eyes.Add(newEye);

    }

}
