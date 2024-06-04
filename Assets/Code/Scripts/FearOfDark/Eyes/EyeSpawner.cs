using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EyeSpawner : MonoBehaviour
{
    private IsVisibleChecker isVisibleChecker;

    [SerializeField] private int maxEyes = 20;
    public List<GameObject> eyes;

    private GameObject player;

    public GameObject eyePrefab;

    [SerializeField] private float distancePerEye = 0.05f;

    [SerializeField] private float initialDelayToSpawnEye = 2;
    [SerializeField] private float delayReductionMultiplier = 0.5f;
    [SerializeField] private float minDelayToSpawnEye = 0.01f;

    private float currentDelayToSpawnEye;

    [SerializeField] private float initialDelayToKillThis = 10;
    [SerializeField] private float delayToKillThis = 2;

    private float currentDelayToKillThis;

    [SerializeField] private GameEvent damagePlayerEvent;
    [SerializeField] private float damageOnEyeAppearance;

    private void Awake() {
        isVisibleChecker = GetComponent<IsVisibleChecker>();
        PlayerWatcherComponent.addSubToPlayerChanged(PlayerChanged);
        player = PlayerWatcherComponent.getPlayer();
    }


    // Start is called before the first frame update
    void Start()
    {

        spawnEye();
        resetDelayToSpawnEye();
        currentDelayToKillThis = initialDelayToKillThis;

    }

    public void PlayerChanged(GameObject newPlayer) {

        player = newPlayer;

    }

    // Update is called once per frame
    void Update()
    {

        currentDelayToKillThis -= Time.deltaTime;
        currentDelayToSpawnEye -= Time.deltaTime;

        if(isVisibleChecker.isVisible()) {

            if (currentDelayToSpawnEye <= 0) {

                if(eyes.Count < maxEyes)
                    spawnEye();
                
                damagePlayerEvent.Raise(this, damageOnEyeAppearance);
                resetDelayToSpawnEye();
            }

            resetDelayToKillThis();

        } else {

            if (currentDelayToKillThis <= 0) {
                Destroy(gameObject);

            } else
                resetDelayToSpawnEye();

        }
        
    }

    private void resetDelayToSpawnEye() {

        currentDelayToSpawnEye = Mathf.Max(initialDelayToSpawnEye / (1 + delayReductionMultiplier * eyes.Count), minDelayToSpawnEye);


    }

    private void resetDelayToKillThis() {


        currentDelayToKillThis = delayToKillThis;


    }

    private void spawnEye() {

        Vector3 betweenEyeAndPlayer = player.transform.position - transform.position;

        Vector3 perpendicular = Vector3.Cross(betweenEyeAndPlayer, transform.up).normalized;

        int randomRotation = Random.Range(0, 180);

        Vector3 vectorToEye = Quaternion.Euler(0, 0, randomRotation) * perpendicular;

        GameObject newEye = Instantiate(eyePrefab, this.transform);

        newEye.transform.position = this.transform.position + vectorToEye * distancePerEye * eyes.Count;

        eyes.Add(newEye);



    }

}
