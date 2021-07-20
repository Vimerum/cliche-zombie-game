using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject popUp;

    public void NewGame() {
        SceneManager.LoadScene(1);
    }

    public void ShowQuitPopUp() {
        popUp.SetActive(true);
    }

    public void HideQuitPopUp() {
        popUp.SetActive(false);
    }

    public void Quit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
