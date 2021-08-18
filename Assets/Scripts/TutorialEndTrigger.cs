using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEndTrigger : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

}
