using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController.Examples;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerPlatforms : MonoBehaviour
{
    public bool isTimerStarted = false;
    public float currentTimer, startingTimer;
    public TextMeshProUGUI currentTimerText;
    public GameObject currentTimerTextObject;
    public ExampleCharacterController exampleCharacterController;
    public void Start()
    {
        exampleCharacterController = FindObjectOfType<ExampleCharacterController>();
    }
    public void StartTimer()
    {
        isTimerStarted = true;
        currentTimer = 0 + startingTimer;
        currentTimerTextObject.SetActive(true);
    }
    public void StopTimer()
    {
        currentTimerTextObject.SetActive(false);
        isTimerStarted = false;
        exampleCharacterController.SetPlatformingCapacityState(false);
    }
    public void Update()
    {
        if(isTimerStarted)
        {
            currentTimer -= Time.deltaTime;
            currentTimerText.text = currentTimer.ToString("f0");
            if(currentTimer < 1f)
                StopTimer();
        }
    }
    public void AddToTimer(float addToTimer)
    {
        currentTimer += addToTimer;
    }
}
