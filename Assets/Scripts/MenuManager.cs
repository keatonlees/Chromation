using UnityEngine;

public class MenuManager : MonoBehaviour {

    public SceneFader sceneFader;  // reference to animation fader

    public void MainMenu() {
        sceneFader.FadeTo("MainMenu");  // go to main menu
    }

    public void LevelSelect() {
        sceneFader.FadeTo("LevelSelect");  // go to level select
    }

    public void SettingsMenu() {
        sceneFader.FadeTo("SettingsMenu");  // go to settings menu
    }

    public void QuitGame() {
        Debug.Log("Quit game!");
        Application.Quit();  // quit game
    }

}
