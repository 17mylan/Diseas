using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FPS : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    private float deltaTime = 0.0f;

    private void Start()
    {
        Application.targetFrameRate = -1;
    }
    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        int roundedFPS = Mathf.RoundToInt(fps);
        fpsText.text = roundedFPS.ToString();
        if (roundedFPS <= 30)
        {
            fpsText.color = Color.red;
        }
        else if (roundedFPS > 31 && roundedFPS <= 59)
        {
            fpsText.color = Color.yellow;
        }
        else if (roundedFPS >= 60)
        {
            fpsText.color = Color.green;
        }
    }
}
