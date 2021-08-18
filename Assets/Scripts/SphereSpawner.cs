using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSpawner : MonoBehaviour {

    Material defaultBaseMaterial;
    Material objectMaterial;

    [SerializeField] LayerMask objectMask;
    RaycastHit isObjectHit;
    bool isObject;
    float objectDistance = 0.4f;

    void Awake() {
        // get original materials of spawner
        defaultBaseMaterial = transform.GetComponent<Renderer>().material;
    }

    void Update() {
        // check if object still on top of base
        isObject = Physics.Raycast(transform.position, Vector3.up, out isObjectHit, objectDistance, objectMask);

        // if no object, reset to original colours
        if (!isObject) {
            transform.GetComponent<Renderer>().material = defaultBaseMaterial;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        // if coloured sphere on top, set colour of base to sphere
        if (collision.gameObject.CompareTag("ColouredObject")) {
            objectMaterial = collision.gameObject.GetComponent<Renderer>().material;
            if (isObject) transform.GetComponent<Renderer>().material = objectMaterial;
        }
    }

}
