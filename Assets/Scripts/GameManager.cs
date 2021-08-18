using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public SceneFader sceneFader;  // reference to animation fader

    public int nextLevelIndex;

    public void CompletedLevel() {
        // gets current highest level and compares it to current level (check for if player is replaying a previous level)
        int currentLevelReached = PlayerPrefs.GetInt("levelReached", 1);
        if (currentLevelReached < nextLevelIndex + 1) PlayerPrefs.SetInt("levelReached", nextLevelIndex + 1);

        Cursor.visible = true;  // show cursor
        Cursor.lockState = CursorLockMode.None;  // unlock cursor
        sceneFader.FadeTo("LevelSelect");
    }

    public void ResetLevels() {
        Debug.Log("Reset all levels");
        PlayerPrefs.SetInt("levelReached", 1);
    }
}
