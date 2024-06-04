using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.XR;

public class GravityAffectedMovement : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float sprintMultiplier = 2;

    [SerializeField] private float groundDrag = 6;
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private float jumpCooldown = 0.25f;
    [SerializeField] private float airMultiplier = 0.4f; //when moving in air, like gliding

    [SerializeField] private bool canJump = true;

    //For input in outside scripts
    [HideInInspector] public bool  isSprinting;
    [HideInInspector] public float verticalInput;
    [HideInInspector] public float horizontalInput;
    
    private Vector3 moveDirection;
    
    private bool readyToJump;

    [Header("Ground Detection")]
    [SerializeField] private float height = 2;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private string movablePlatformTag;

    private Transform trueParent; //the normal parent of this object, can be null


    private bool grounded; //if is touching ground

    Rigidbody rb;

    public MovementState state;


    public enum MovementState {

        idle,
        walking,
        sprinting,
        airborne
    }


    // Start is called before the first frame update
    void Start() {

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        
        trueParent = transform.parent;

    }

    // Update is called once per frame
    void Update() {

        groundCheck();
        StateHandler();
        move();

        if (grounded) 
            rb.drag = groundDrag;

        else
            rb.drag = 0;
        
        //debug position
    }

    private void StateHandler() {


        // if we're grounded
        if (grounded) {

            //if we're moving
            if (verticalInput != 0 || horizontalInput != 0) {

                if (isSprinting)
                    state = MovementState.sprinting;

                else
                    state = MovementState.walking;

            } else
                state = MovementState.idle;

            // Mode - Air
        } else
            state = MovementState.airborne;
        
    }

    public void tryJump() {

        if (canJump && readyToJump && grounded) {

            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);

        }

    }


    private void move() {


        moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;


        switch (state) {

            case MovementState.sprinting:

                rb.velocity = moveDirection.normalized * moveSpeed * sprintMultiplier + new Vector3(0, rb.velocity.y, 0);

                break;

            case MovementState.walking:

                rb.velocity = moveDirection.normalized * moveSpeed + new Vector3(0, rb.velocity.y, 0);

                break;

            case MovementState.idle:

                break;

            case MovementState.airborne:

                transform.parent = trueParent;


                rb.velocity = moveDirection.normalized * moveSpeed * airMultiplier + new Vector3(0, rb.velocity.y, 0);


                break;


        }


    }

    private void groundCheck() {

        RaycastHit hit;

        grounded = Physics.Raycast(transform.position, Vector3.down, out hit, height * 0.5f + 0.2f, groundLayer);

        if (grounded && hit.transform.tag.Equals(movablePlatformTag))
                transform.parent = hit.collider.transform.parent;
        

    }


    private void Jump() {


        state = MovementState.airborne;

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        transform.position += new Vector3(0, 0.01f, 0);

        transform.parent = trueParent;
    }

    private void ResetJump() {
        readyToJump = true;
    }


}
