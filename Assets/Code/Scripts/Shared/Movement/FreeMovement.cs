using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class FreeMovement : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float sprintMultiplier = 2;


    //For input in outside scripts
    [HideInInspector] public bool  isSprinting;
    [HideInInspector] public float verticalInput;
    [HideInInspector] public float horizontalInput;
    
    private Vector3 moveDirection;

    // Update is called once per frame
    void Update() {

        move();

    }


    private void move() {

        
        moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
«

        if (isSprinting) {

            transform.position +=  moveDirection.normalized * moveSpeed * sprintMultiplier * Time.deltaTime;


        } else {


            transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;

        }


    }

}
