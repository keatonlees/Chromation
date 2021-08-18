using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : MonoBehaviour {
    public GameManager gameManager;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") gameManager.CompletedLevel();
    }
}
