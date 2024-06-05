using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(GravityAffectedMovement))]
public class PlayerController : MonoBehaviour
{

    //player movement stuff
    protected GravityAffectedMovement movementController;

    [Header("Cameras")]

    [SerializeReference] public GameObject firstPersonCamera;

    [SerializeReference] public GameObject freeMovingCamera;
    protected FreeMovement freeMovingCameraController;

    protected CameraScript currentCameraScript;

    protected bool freeMovingCameraMode = false;

    protected Rigidbody rb;

    protected bool paused = false;

    [Header("Events")]

    [SerializeReference] protected GameEvent pauseEvent;
    [SerializeReference] protected GameEvent playerAnnouncedEvent;

    [Header("Sounds")]

    [SerializeReference] protected SFXSoundComponent placeholderSoundComponent;

    // Start is called before the first frame update
    protected void Awake()
    {
        movementController = GetComponent<GravityAffectedMovement>();

        freeMovingCameraController = freeMovingCamera.GetComponent<FreeMovement>();

        rb = GetComponent<Rigidbody>();

        currentCameraScript = firstPersonCamera.GetComponent<CameraScript>();


        freeMovingCameraMode = false;
        

    }

    protected void Start() {

        playerAnnouncedEvent.Raise(this, null);

    }

    protected void OnEnable() {

        playerAnnouncedEvent.Raise(this, null);

    }

    // Update is called once per frame
    protected void Update()
    {

        handleInput();
        
    }


    protected void handleInput() {

        if (!freeMovingCameraMode) {

            if(!paused) {

                if (Input.GetKey(KeyCode.Space))
                    movementController.tryJump();

                if (Input.GetKey(KeyCode.LeftShift))
                    movementController.isSprinting = true;

                else
                    movementController.isSprinting = false;

                if (Input.GetKey("1"))
                    LevelsManager.instance.transitionToLevel1();

                else if (Input.GetKey("2"))
                    LevelsManager.instance.transitionToLevel2();

                else if (Input.GetKey("3"))
                    ;//changeToLevelThreeEvent.Raise(this, null);

                movementController.horizontalInput = Input.GetAxis("Horizontal");
                movementController.verticalInput = Input.GetAxis("Vertical");

                currentCameraScript.rotateCamera(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));


            }

            if (Input.GetKeyDown(KeyCode.U))
                switchToFreeMovingCamera();

        }else {


            if (Input.GetKey(KeyCode.LeftShift))
                freeMovingCameraController.isSprinting = true;

            else
                freeMovingCameraController.isSprinting = false;

            freeMovingCameraController.horizontalInput = Input.GetAxis("Horizontal");
            freeMovingCameraController.verticalInput = Input.GetAxis("Vertical");


            if (Input.GetKeyDown(KeyCode.U))
                switchFromFreeMovingCamera();

            currentCameraScript.rotateCamera(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));



        }

        if (Input.GetKeyDown(KeyCode.P))
            pauseEvent.Raise(this, !paused);


    }

    protected void switchToFreeMovingCamera() {

        freeMovingCameraMode = true;

        movementController.enabled = false;

        firstPersonCamera.SetActive(false);
        freeMovingCamera.SetActive(true);

        currentCameraScript = freeMovingCamera.GetComponent<CameraScript>();


    }

    protected void switchFromFreeMovingCamera() {

        freeMovingCameraMode = false;

        movementController.enabled = true;

        firstPersonCamera.SetActive(true);
        freeMovingCamera.SetActive(false);

        currentCameraScript = firstPersonCamera.GetComponent<CameraScript>();

    }

    public void onPauseEvent(Component sender, object data) {

        bool toPause = (bool)data;

        if (toPause)
            pausePlayer();

        else
            unpausePlayer();

    }

    protected void pausePlayer() {

        paused = true;
        rb.isKinematic = true;
        movementController.enabled = false;


    }

    protected void unpausePlayer() {

        paused = false;
        rb.isKinematic = false;
        movementController.enabled = true;


    }

}
