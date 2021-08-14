using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject quitPopUp;
    public GameObject controlPopUp;

    public void NewGame() {
        SceneManager.LoadScene(1);
    }

    public void ShowControlsPopUp() {
        controlPopUp.SetActive(true);
    }

    public void HideControlsPopUp() {
        controlPopUp.SetActive(false);
    }

    public void ShowQuitPopUp() {
        quitPopUp.SetActive(true);
    }

    public void HideQuitPopUp() {
        quitPopUp.SetActive(false);
    }

    public void Quit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
