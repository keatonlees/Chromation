using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour {

    public SceneFader sceneFader;  // reference to animation fader
    public Button[] levelButtons;  // array of the buttons

    void Start() {
        // gets highest level completed
        int levelReached = PlayerPrefs.GetInt("levelReached", 1);

        // highlights buttons based on what player has completed
        for (int i = 0; i < levelButtons.Length; i++) if (i + 1 > levelReached) levelButtons[i].interactable = false;
    }

    public void MainMenu() {
        sceneFader.FadeTo("MainMenu");  // go to main menu
    }

    public void SelectLevel(string levelName) {
        sceneFader.FadeTo(levelName);  // load level
    }
}
