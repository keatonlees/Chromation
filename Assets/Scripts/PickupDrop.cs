using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupDrop : MonoBehaviour {

    [SerializeField] LayerMask pickupMask;
    [SerializeField] Transform holdParent;
    [SerializeField] GameObject tooltipUI;
    private GameObject heldObject;
    float pickupRange = 5f;
    float moveForce = 250f;
    float worldScale = 1f;
    float heldScale = 0.6f;
    bool isObject = false;

    void Update() {
        // check for player looking at object
        RaycastHit objectHit;
        isObject = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out objectHit, pickupRange, pickupMask);

        // if no object current in hand
        if (heldObject == null) {

            // if looking at pickupable object
            if (isObject) {

                // show tooltip
                tooltipUI.SetActive(true);

                // if player presses 'E', pickup the object
                if (Input.GetKeyDown(KeyCode.E)) PickupObject(objectHit.transform.gameObject);
            } else {

                // if not looking at object, hide tooltip
                tooltipUI.SetActive(false);
            }
        } else {
            // if holding object, hide tooltip and move object
            tooltipUI.SetActive(false);
            MoveObject();

            // if player presses 'E', drop the object
            if (Input.GetKeyDown(KeyCode.E)) DropObject();
        }
    }

    void MoveObject() {
        if (Vector3.Distance(heldObject.transform.position, holdParent.position) > 0.1f) {
            Vector3 moveDirection = (holdParent.position - heldObject.transform.position);
            heldObject.GetComponent<Rigidbody>().AddForce(moveDirection * moveForce);
        }
    }

    void PickupObject(GameObject pickupObject) {
        if (pickupObject.GetComponent<Rigidbody>()) {
            Rigidbody objectRB = pickupObject.GetComponent<Rigidbody>();
            objectRB.useGravity = false;
            objectRB.drag = 25f;
            objectRB.freezeRotation = true;
            objectRB.transform.localScale = new Vector3(heldScale, heldScale, heldScale);

            objectRB.transform.parent = holdParent;
            heldObject = pickupObject;
        }
    }

    void DropObject() {
        Rigidbody heldRB = heldObject.GetComponent<Rigidbody>();
        heldRB.useGravity = true;
        heldRB.drag = 1f;
        heldRB.freezeRotation = false;
        heldRB.transform.localScale = new Vector3(worldScale, worldScale, worldScale);

        heldObject.transform.parent = null;
        heldObject = null;
    }

}
