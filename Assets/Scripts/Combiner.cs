using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combiner : MonoBehaviour {

    [SerializeField] Camera playerCam;

    [SerializeField] GameObject[] wires;
    [SerializeField] Transform input1Base;
    [SerializeField] Transform input2Base;
    [SerializeField] Transform outputBase;
    [SerializeField] Transform button;

    Material input1Material;
    Material input2Material;
    Color mixedColour;

    [SerializeField] LayerMask objectMask;
    RaycastHit input1Hit;
    RaycastHit input2Hit;
    bool isInput1;
    bool isInput2;
    float objectDistance = 0.17f;

    Color defaultOutputColour;
    Color defaultWireColour;
    Color defaultButtonColour;

    [SerializeField] Material correctMaterial;

    [SerializeField] LayerMask buttonMask;
    float buttonRange = 5f;

    bool isButton = false;
    [SerializeField] GameObject tooltipUI;

    void Awake() {
        // get default colours
        defaultWireColour = wires[0].GetComponent<Renderer>().material.color;
        defaultOutputColour = outputBase.GetComponent<Renderer>().material.color;
        defaultButtonColour = button.GetComponent<Renderer>().material.color;
    }

    void Update() {
        if (CheckObjects()) {
            // get the materials of the inputs (cheating a little because of getting the colours from the base, not the sphere)
            input1Material = input1Base.GetComponent<Renderer>().material;
            input2Material = input2Base.GetComponent<Renderer>().material;

            // mix the colours
            mixedColour = MixColours(input1Material.color, input2Material.color);

            // apply the colour as a preview to the wires and output base
            foreach (GameObject wire in wires) {
                wire.GetComponent<Renderer>().material.color = mixedColour;
            }
            outputBase.GetComponent<Renderer>().material.color = mixedColour;
            button.GetComponent<Renderer>().material.color = correctMaterial.color;

            RaycastHit buttonHit;
            isButton = Physics.Raycast(playerCam.transform.position, playerCam.transform.TransformDirection(Vector3.forward), out buttonHit, buttonRange, buttonMask);

            // if the player hits the button, combine the spheres
            if (isButton) {
                tooltipUI.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E)) CombineObjects();
            } else {
                tooltipUI.SetActive(false);
            }

        } else {
            // else reset the wires and output base to the original colours
            foreach (GameObject wire in wires) {
                wire.GetComponent<Renderer>().material.color = defaultWireColour;
            }
            outputBase.GetComponent<Renderer>().material.color = defaultOutputColour;
            button.GetComponent<Renderer>().material.color = defaultButtonColour;

            tooltipUI.SetActive(false);
        }
    }

    void CombineObjects() {
        // destroy old spheres
        Destroy(input1Hit.transform.gameObject);
        Destroy(input2Hit.transform.gameObject);

        // create combined sphere
        GameObject combinedObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        combinedObject.AddComponent<Rigidbody>();
        combinedObject.tag = "ColouredObject";
        combinedObject.layer = 13;
        combinedObject.GetComponent<Renderer>().material.color = mixedColour;
        combinedObject.transform.position = new Vector3(outputBase.transform.position.x, outputBase.transform.position.y + 1f, outputBase.transform.position.z);
    }

    bool CheckObjects() {
        // if there is a sphere on both inputs return true, else return false
        isInput1 = Physics.Raycast(input1Base.transform.position, transform.up, out input1Hit, objectDistance, objectMask);
        isInput2 = Physics.Raycast(input2Base.transform.position, transform.up, out input2Hit, objectDistance, objectMask);
        if (isInput1 && isInput2) return true;
        return false;
    }

    Color MixColours(Color colour1, Color colour2) {
        // combine rgb values
        double r = (colour1.r + colour2.r) / 2;
        double g = (colour1.g + colour2.g) / 2;
        double b = (colour1.b + colour2.b) / 2;
        return (new Color((float)Math.Round(r, 3), (float)Math.Round(g, 3), (float)Math.Round(b, 3)));
    }

}
