using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStartTrigger : MonoBehaviour {

    [SerializeField] GameObject tutorialUI;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") tutorialUI.SetActive(true);
    }
}
