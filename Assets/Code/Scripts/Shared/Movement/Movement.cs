using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float sprintMultiplier = 2;

    [SerializeField] private float groundDrag = 6;
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private float jumpCooldown = 0.25f;
    [SerializeField] private float airMultiplier = 0.4f; //when moving in air, like gliding


    //For input in outside scripts
    [HideInInspector] public bool isSprinting;
    [HideInInspector] public float verticalInput;
    [HideInInspector] public float horizontalInput;
    
    private Vector3 moveDirection;
    
    private bool readyToJump;

    [Header("Ground Detection")]
    [SerializeField] private float height = 2;
    
    private bool grounded; //if is touching ground

    Rigidbody rb;

    public MovementState state;

    public enum MovementState {
        walking,
        sprinting,
        airborne
    }


    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
    }

    // Update is called once per frame
    void Update() {

        groundCheck();
        MyInput();
        StateHandler();
        MovePlayer();

        if (grounded) 
            rb.drag = groundDrag;

        else
            rb.drag = 0;
        
        //debug position
    }

    private void StateHandler() {


        // Mode - Walking
        if (grounded) {

            if(isSprinting)
                state = MovementState.sprinting;

            else
                state = MovementState.walking;


            // Mode - Air
        } else
            state = MovementState.airborne;
        
    }


    private void MyInput() {

        if (Input.GetKey(KeyCode.Space))
            tryJump();

        if (Input.GetKey(KeyCode.LeftShift))
            isSprinting = true;

        else
            isSprinting = false;

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

    }

    private void tryJump() {

        Debug.Log("Trying to jump: " + readyToJump);

        if (readyToJump && grounded) {

            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);

        }

    }


    private void MovePlayer() {


        moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;

        if (state == MovementState.walking) {

            rb.velocity = moveDirection.normalized * moveSpeed + new Vector3(0, rb.velocity.y, 0);

            Debug.Log("Walking " + rb.velocity);


        } else if (state == MovementState.sprinting) {

            rb.velocity = moveDirection.normalized * moveSpeed * sprintMultiplier + new Vector3(0, rb.velocity.y, 0);

            Debug.Log("Sprinting " + rb.velocity);


        } else {

            rb.velocity = moveDirection.normalized * moveSpeed * airMultiplier + new Vector3(0, rb.velocity.y, 0);

            Debug.Log("Gliding " + rb.velocity);

        }


    }

    private void groundCheck() {
        grounded = Physics.Raycast(transform.position, Vector3.down, height * 0.5f + 0.2f);
    }


    private void Jump() {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump() {
        readyToJump = true;
    }


}
