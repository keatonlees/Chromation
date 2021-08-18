using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour {

    public SceneFader sceneFader;  // reference to animation fader
    public static bool isPaused = false;  // global variable for checking
    public GameObject pauseMenuUI;  // pause menu canvas
    public GameObject player;  // player rb

    void Update() {
        // toggles pause menu
        if (Input.GetKeyDown(KeyCode.P)) {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void ResumeGame() {
        pauseMenuUI.SetActive(false);  // hide pause menu
        Time.timeScale = 1f;  // start game runtime
        isPaused = false;

        Cursor.visible = false;  // hide cursor
        Cursor.lockState = CursorLockMode.Locked;  // lock cursor
        // enable player scripts
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<PlayerCamera>().enabled = true;
    }

    void PauseGame() {
        pauseMenuUI.SetActive(true);  // show pause menu
        Time.timeScale = 0f;  // stop game runtime
        isPaused = true;

        Cursor.visible = true;  // show cursor
        Cursor.lockState = CursorLockMode.None;  // unlock cursor
        // disable player scripts
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<PlayerCamera>().enabled = false;
    }

    public void RestartLevel() {
        Time.timeScale = 1f;  // start game runtime
        isPaused = false;
        sceneFader.FadeTo(SceneManager.GetActiveScene().name);  // reload the level
    }

    public void MainMenu() {
        Time.timeScale = 1f;  // start game runtime
        isPaused = false;
        sceneFader.FadeTo("MainMenu");  // go to main menu
    }

}
