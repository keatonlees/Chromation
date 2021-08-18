using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour {

    [SerializeField] Transform ring;
    [SerializeField] Material correctMaterial;
    Color defaultRingColor;
    Color defaultWireColour;

    [SerializeField] LayerMask objectMask;
    RaycastHit isObjectHit;
    bool isObject = false;
    float objectDistance = 0.4f;

    Color baseMaterialColour;
    Color objectMaterialColour;

    [SerializeField] GameObject door;
    Animator doorAnimator;

    [SerializeField] GameObject[] wires;

    void Awake() {
        // get original material of ring
        defaultRingColor = ring.transform.GetComponent<Renderer>().material.color;
        defaultWireColour = wires[0].GetComponent<Renderer>().material.color;
        doorAnimator = door.transform.GetComponent<Animator>();
    }

    void Update() {
        // check if object still on top of base
        isObject = Physics.Raycast(transform.position, Vector3.up, out isObjectHit, objectDistance, objectMask);

        // if no object, reset to original colours
        if (!isObject) {
            ring.transform.GetComponent<Renderer>().material.color = defaultRingColor;
            foreach (GameObject wire in wires) {
                wire.GetComponent<Renderer>().material.color = defaultWireColour;
            }
            doorAnimator.SetBool("isOpening", false);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("ColouredObject")) {
            baseMaterialColour = transform.GetComponent<Renderer>().material.color;
            objectMaterialColour = collision.gameObject.GetComponent<Renderer>().material.color;

            if (isObject && (objectMaterialColour == baseMaterialColour)) {
                // correct object
                ring.transform.GetComponent<Renderer>().material.color = correctMaterial.color;
                foreach (GameObject wire in wires) {
                    wire.GetComponent<Renderer>().material.color = correctMaterial.color;
                }
                doorAnimator.SetBool("isOpening", true);
            }
        }
    }

}
