using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(GravityAffectedMovement))]
public class PlayerController : MonoBehaviour
{

    //player movement stuff
    private GravityAffectedMovement movementController;

    public GameObject firstPersonCamera;

    public GameObject freeMovingCamera;
    private FreeMovement freeMovingCameraController;

    private bool freeMovingCameraMode = false;


    // Start is called before the first frame update
    void Awake()
    {
        movementController = GetComponent<GravityAffectedMovement>();

        freeMovingCameraController = freeMovingCamera.GetComponent<FreeMovement>();


        freeMovingCameraMode = false;
        

    }

    // Update is called once per frame
    void Update()
    {

        handleInput();
        
    }


    private void handleInput() {

        if (!freeMovingCameraMode) {

            if (Input.GetKey(KeyCode.Space))
                movementController.tryJump();

            if (Input.GetKey(KeyCode.LeftShift))
                movementController.isSprinting = true;

            else
                movementController.isSprinting = false;

            movementController.horizontalInput = Input.GetAxis("Horizontal");
            movementController.verticalInput = Input.GetAxis("Vertical");


            if(Input.GetKeyDown(KeyCode.U))
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


        }

    }

    private void switchToFreeMovingCamera() {

        freeMovingCameraMode = true;

        movementController.enabled = false;

        firstPersonCamera.SetActive(false);
        freeMovingCamera.SetActive(true);


    }

    private void switchFromFreeMovingCamera() {

        freeMovingCameraMode = false;

        movementController.enabled = true;

        firstPersonCamera.SetActive(true);
        freeMovingCamera.SetActive(false);

    }

}
