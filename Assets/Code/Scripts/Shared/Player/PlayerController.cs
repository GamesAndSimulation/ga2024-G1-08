using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Movement))]
public class PlayerController : MonoBehaviour
{

    private Movement movementController;


    // Start is called before the first frame update
    void Awake()
    {
        movementController = GetComponent<Movement>();
        
    }

    // Update is called once per frame
    void Update()
    {

        handleInput();
        
    }


    private void handleInput() {


        if (Input.GetKey(KeyCode.Space))
            movementController.tryJump();

        if (Input.GetKey(KeyCode.LeftShift))
            movementController.isSprinting = true;

        else
            movementController.isSprinting = false;

        movementController.horizontalInput = Input.GetAxis("Horizontal");
        movementController.verticalInput = Input.GetAxis("Vertical");

    }

}
