using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdminManager : MonoBehaviour {

    public GameManager gameManager;

    void Update() {
        if (Input.GetKeyDown(KeyCode.End)) gameManager.CompletedLevel();
        if (Input.GetKeyDown(KeyCode.Home)) gameManager.ResetLevels();
    }
}
