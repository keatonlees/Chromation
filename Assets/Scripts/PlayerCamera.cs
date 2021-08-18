using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    [Header("Mouse Settings")]
    [SerializeField] Transform cam;  // reference to main camera
    [SerializeField] Transform orientation;  // reference to player direction
    [SerializeField] WallRun wallRun;  // get reference to wall run for camera tilt

    [Header("Mouse Settings")]
    float mouseX;
    float mouseY;
    float xRotation;
    float yRotation;
    float sensitivityX = 1f;
    float sensitivityY = 1f;
    float sensitivityMultipler = 1.5f;  // will change for both X and Y, can be used with sensitvity slider

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;  // lock cursor to middle of screen
        Cursor.visible = false;  // hide cursor
    }

    void Update() {
        GetInputs();  // get mouse inputs
        
        // move camera
        cam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, wallRun.tilt);
        orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    void GetInputs() {
        mouseX = Input.GetAxisRaw("Mouse X");  
        mouseY = Input.GetAxisRaw("Mouse Y");   
        yRotation += mouseX * sensitivityX * sensitivityMultipler;
        xRotation -= mouseY * sensitivityY * sensitivityMultipler;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);  // limits player so they cannot look more than straight down and straight up
    }
}
