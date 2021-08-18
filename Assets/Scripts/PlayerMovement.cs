using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [Header("References")]
    [SerializeField] Transform orientation;  // reference to player direction
    Rigidbody rb;  // reference to player

    [Header("Movement")]
    Vector3 movementDirection;
    float horizontalMovement;
    float verticalMovement;
    float extraGravity = 10f;  // jumping snapiness down
    float movementSpeed = 40f;  // how fast the player moves
    float movementMultiplier = 1f;  // if wanting to implement sprinting
    float airMultiplier = 0.4f;  // player moves slower in air
    float maxSpeed = 10f;  // max speed in all directions, including diagonal

    [Header("Jumping")]
    bool jumpPressed = false;
    bool readyToJump = true;
    float jumpForce = 75f;  // how high a player jumps
    float jumpCooldown = 0.5f;  // how long until the player can jump again

    [Header("Counter Movement")]
    float counterMovement = 0.5f;  // for opposite direction movement snapiness
    float threshold = 0.01f;

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    bool isGrounded;
    float groundDistance = 0.5f;

    [Header("Wall Detection")]
    [SerializeField] Transform wallCheck;
    [SerializeField] LayerMask wallMask;
    RaycastHit frontWallHit;
    RaycastHit backWallHit;
    RaycastHit leftWallHit;
    RaycastHit rightWallHit;
    bool wallFront = false;
    bool wallBack = false;
    bool wallLeft = false;
    bool wallRight = false;
    float wallDistance = 0.51f;

    void Start() {
        rb = GetComponent<Rigidbody>();  // get player rb
        rb.freezeRotation = true;  // so rb doesn't ragdoll
    }

    void Update() {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);  // check for ground
        GetInputs();  // get keyboard inputs
        CheckWall();  // check for walls
    }

    void FixedUpdate() {
        MovePlayer();  // in FixedUpdate because this is physics based
    }

    void GetInputs() {
        horizontalMovement = Input.GetAxisRaw("Horizontal");  // keyboard ad
        verticalMovement = Input.GetAxisRaw("Vertical");  // keyboard ws
        jumpPressed = Input.GetButton("Jump");  // spacebar
    }

    void CheckWall() {
        wallLeft = Physics.Raycast(wallCheck.position, -orientation.right, out leftWallHit, wallDistance, wallMask);
        wallRight = Physics.Raycast(wallCheck.position, orientation.right, out rightWallHit, wallDistance, wallMask);
        wallFront = Physics.Raycast(wallCheck.position, orientation.forward, out frontWallHit, wallDistance, wallMask);
        wallBack = Physics.Raycast(wallCheck.position, -orientation.forward, out backWallHit, wallDistance, wallMask);
    }

    void MovePlayer() {
        // extra gravity
        rb.AddForce(Vector3.down * extraGravity, ForceMode.Acceleration);

        // get current relvative velocities
        Vector2 magnitude = GetRelativeVelocity();
        float xMagnitude = magnitude.x;
        float yMagnitude = magnitude.y;

        // counter movement to decrease sliding
        CounterMovement(horizontalMovement, verticalMovement, magnitude);

        if (jumpPressed && readyToJump) Jump();

        // limit headbanging into wall and buggy wall movement
        if (horizontalMovement > 0 && wallRight && rightWallHit.normal.y == 0) horizontalMovement = 0;
        if (horizontalMovement < 0 && wallLeft && leftWallHit.normal.y == 0) horizontalMovement = 0;
        if (verticalMovement > 0 && wallFront && frontWallHit.normal.y == 0) verticalMovement = 0;
        if (verticalMovement < 0 && wallBack && backWallHit.normal.y == 0) verticalMovement = 0;

        // limit max speed
        if (horizontalMovement > 0 && xMagnitude > maxSpeed) horizontalMovement = 0;
        if (horizontalMovement < 0 && xMagnitude < -maxSpeed) horizontalMovement = 0;
        if (verticalMovement > 0 && yMagnitude > maxSpeed) verticalMovement = 0;
        if (verticalMovement < 0 && yMagnitude < -maxSpeed) verticalMovement = 0;

        // apply movement forces
        movementDirection = orientation.transform.forward * verticalMovement + orientation.transform.right * horizontalMovement;
        if (isGrounded) rb.AddForce(movementDirection.normalized * movementSpeed * movementMultiplier, ForceMode.Acceleration);
        else rb.AddForce(movementDirection.normalized * movementSpeed * airMultiplier, ForceMode.Acceleration);
    }

    void Jump() {
        if (isGrounded) {
            readyToJump = false;

            // add jump forces
            rb.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
            Vector3 velocity = rb.velocity;
            if (rb.velocity.y < 0.05f) rb.velocity = new Vector3(velocity.x, 0, velocity.z);
            else if (rb.velocity.y > 0) rb.velocity = new Vector3(velocity.x, velocity.y / 2, velocity.z);

            // reset after cooldown
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump() {
        readyToJump = true;
    }

    void CounterMovement(float x, float y, Vector2 magnitude) {
        // if jump pressed, in air
        if (jumpPressed) return;

        // add opposite forces
        if (Mathf.Abs(magnitude.x) > threshold && Mathf.Abs(x) < 0.05f || (magnitude.x < -threshold && x > 0) || (magnitude.x > threshold && x < 0))
            rb.AddForce(orientation.transform.right * movementSpeed * -magnitude.x * counterMovement);
        if (Mathf.Abs(magnitude.y) > threshold && Mathf.Abs(y) < 0.05f || (magnitude.y < -threshold && y > 0) || (magnitude.y > threshold && y < 0))
            rb.AddForce(orientation.transform.forward * movementSpeed * -magnitude.y * counterMovement);

        // limit diagonal max speed
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.y, 2))) > maxSpeed) {
            Vector3 normalizedSpeed = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(normalizedSpeed.x, rb.velocity.y, normalizedSpeed.z);
        }
    }

    public Vector2 GetRelativeVelocity() {
        // get relative velocity to where player is facing
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;
        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;
        float magnitude = rb.velocity.magnitude;
        float xMagnitude = magnitude * Mathf.Cos(v * Mathf.Deg2Rad);
        float yMagnitude = magnitude * Mathf.Cos(u * Mathf.Deg2Rad);
        return new Vector2(xMagnitude, yMagnitude);
    }
}
