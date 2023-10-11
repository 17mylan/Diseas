using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTransition : MonoBehaviour
{
    public GameObject white, black;
    public void DisableBlackAndActivateWhite()
    {
        black.SetActive(false);
        white.SetActive(true);
    }
    public void DisableWhite()
    {
        white.SetActive(false);
    }
}
