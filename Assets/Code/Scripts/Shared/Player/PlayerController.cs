using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(GravityAffectedMovement))]
public class PlayerController : MonoBehaviour
{

    //player movement stuff
    private GravityAffectedMovement movementController;

    [Header("Cameras")]

    [SerializeReference] public GameObject firstPersonCamera;

    [SerializeReference] public GameObject freeMovingCamera;
    private FreeMovement freeMovingCameraController;

    private CameraScript currentCameraScript;

    private bool freeMovingCameraMode = false;

    private Rigidbody rb;

    private bool paused = false;

    [Header("Events")]

    [SerializeReference] private GameEvent pauseEvent;
    [SerializeReference] private GameEvent playerAnnouncedEvent;

    // Start is called before the first frame update
    void Awake()
    {
        movementController = GetComponent<GravityAffectedMovement>();

        freeMovingCameraController = freeMovingCamera.GetComponent<FreeMovement>();

        rb = GetComponent<Rigidbody>();

        currentCameraScript = firstPersonCamera.GetComponent<CameraScript>();


        freeMovingCameraMode = false;
        

    }

    private void Start() {

        playerAnnouncedEvent.Raise(this, null);

    }

    // Update is called once per frame
    void Update()
    {

        handleInput();
        
    }


    private void handleInput() {

        if (!freeMovingCameraMode) {

            if(!paused) {

                if (Input.GetKey(KeyCode.Space))
                    movementController.tryJump();

                if (Input.GetKey(KeyCode.LeftShift))
                    movementController.isSprinting = true;

                else
                    movementController.isSprinting = false;

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

    private void switchToFreeMovingCamera() {

        freeMovingCameraMode = true;

        movementController.enabled = false;

        firstPersonCamera.SetActive(false);
        freeMovingCamera.SetActive(true);

        currentCameraScript = freeMovingCamera.GetComponent<CameraScript>();


    }

    private void switchFromFreeMovingCamera() {

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

    private void pausePlayer() {

        paused = true;
        rb.isKinematic = true;
        movementController.enabled = false;


    }

    private void unpausePlayer() {

        paused = false;
        rb.isKinematic = false;
        movementController.enabled = true;


    }

}
