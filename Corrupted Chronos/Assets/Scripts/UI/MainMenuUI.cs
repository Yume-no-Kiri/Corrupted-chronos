using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuCS : MonoBehaviour
{
    [SerializeField] private settingsMenu options;

    public void PlayGame()
    {
        Debug.Log("Play pressed");
        SceneManager.LoadScene("JOCESCENA"); //posar nom escena jugar
    }

    public void EnterConfig()
    {
        options.gameObject.SetActive(true);
        //options.PauseFromStart();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
 
}