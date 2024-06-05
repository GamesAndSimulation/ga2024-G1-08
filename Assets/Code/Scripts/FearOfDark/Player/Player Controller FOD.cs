using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerControllerFOD : MonoBehaviour {


    private GravityAffectedMovement movementScript;

    [Header("State")]

    [Range(0, 1)] public float damage;

    [Range(0, 1)] public float damageRegen = 0.01f;

    [Header("Sounds")]
    [SerializeField] protected SFXSoundComponent stepsSoundComponent;
    [SerializeField] protected float delayToHearSteps = 0.5f;
    [SerializeField] protected float delayToHearStepsSprinting = 0.5f;
    private bool playSteps;

    [Header("Status related sounds")]
    
    [SerializeField] protected FODPlayerSound heartSoundController = new FODPlayerSound(null, new Vector2(0.85f, 1.5f), new Vector2(0.01f, 0.2f));
    [SerializeField] protected FODPlayerSound breathingSoundController = new FODPlayerSound(null, new Vector2(0.85f, 1.0f), new Vector2(0.05f, 0.25f));

    [Header("Events")]
    [SerializeField] protected GameEvent turnOffPlayer;

    [SerializeField] protected AnimationClip turnOffPlayerAnimation;

    protected void Awake() {
        movementScript = GetComponent<GravityAffectedMovement>();
    }

    protected void Start() {

        setupPlayerSounds();
        playSteps = true;

    }
    protected void OnEnable() {
        playSteps = true;
    }

    public void setupPlayerSounds() {

        heartSoundController.setup();
        breathingSoundController.setup();

    }

    // Update is called once per frame
    protected void Update() {

        heartSoundController.updateSound(damage, damage);
        heartSoundController.updateSound(damage, damage);

        damage = Mathf.Max(damage - damageRegen * Time.deltaTime, 0);

        if (playSteps) {

            if (movementScript.state == GravityAffectedMovement.MovementState.sprinting) {

                stepsSoundComponent.PlaySound();
                playSteps = false;
                Invoke(nameof(resetCanPlaySteps), delayToHearStepsSprinting);

            } else if (movementScript.state != GravityAffectedMovement.MovementState.idle) {

                stepsSoundComponent.PlaySound();
                playSteps = false;
                Invoke(nameof(resetCanPlaySteps), delayToHearSteps);

            }

        }

    }

    public void damagePlayer(float damage) {

        this.damage = Mathf.Min(1, damage + this.damage); 

        if(damage >= 1) {

            StartCoroutine(loseAfterAnimation());

        }

    }

    public IEnumerator loseAfterAnimation() {

        yield return new WaitForSeconds(turnOffPlayerAnimation.length + 0.01f);
        LevelsManager.instance.transitionToBadEndingCutscene();

    }

    public void OnPlayerDamaged(Component sender, object data) {

        damagePlayer((float)data);

    }

    public void resetCanPlaySteps() {

        playSteps = true;
    }

}
