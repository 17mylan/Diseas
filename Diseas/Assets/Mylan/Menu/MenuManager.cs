using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public string tutorialSceneName = "TutorialRoom";
    public float delay = 1f;
    public GameObject fadeToBlack;
    private bool hasStartedLoadScene = false;
    public void StartGame()
    {
        if(!hasStartedLoadScene)
            StartCoroutine(StartGameWithDelay());
    }
    IEnumerator StartGameWithDelay()
    {
        fadeToBlack.SetActive(true);
        hasStartedLoadScene = true;
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(tutorialSceneName);
    }
}
