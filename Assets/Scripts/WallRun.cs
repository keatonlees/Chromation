using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour {

    [Header("References")]
    [SerializeField] Transform orientation;  // reference to player direction
    [SerializeField] private Camera cam;  // reference to cam to add tilt
    private Rigidbody rb;  // reference to player

    [Header("Wall Running")]
    float wallRunGravity = 5f;  // gravity while wall running
    float wallRunJumpForce = 30f;  // how much a player jumps out

    [Header("Wall Detection")]
    [SerializeField] LayerMask wallrunMask;
    RaycastHit leftWallHit;
    RaycastHit rightWallHit;
    bool wallLeft = false;
    bool wallRight = false;
    float wallDistance = 0.6f;  // how far the wall is away
    float minJumpHeight = 1.5f;  // how high off the ground player has to be 

    [Header("Camera Tilt")]
    private float camTilt = 10f;  // how much tilt
    private float camTiltTime = 20f;  // how much time to tilt
    public float tilt { get; private set; }

    [Header("Keybinds")]
    KeyCode wallRunKey = KeyCode.Space;  // make same as jump key

    private void Start() {
        rb = GetComponent<Rigidbody>();  // get player rb
    }

    bool CanWallRun() {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight);  // check if player high enough off ground
    }

    void CheckWall() {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance, wallrunMask);  // check if wall on left
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance, wallrunMask);  // check if wall on right
    }

    private void Update() {
        CheckWall();

        if (CanWallRun()) {
            if (wallLeft || wallRight) StartWallRun();
            else StopWallRun();
        } else StopWallRun();
    }

    void StartWallRun() {
        rb.useGravity = false;  // remove normal gravity
        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);  // apply custom gravity

        if (wallLeft) tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);  // tilt camera right
        else if (wallRight) tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);  // tilt camera left

        if (Input.GetKeyDown(wallRunKey)) {
            if (wallLeft) {
                Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
            } else if (wallRight) {
                Vector3 wallRunJumpDirection = transform.up + rightWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
            }
        }
    }

    void StopWallRun() {
        rb.useGravity = true;  // add original gravity
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);  // set camera back to normal
    }

}
