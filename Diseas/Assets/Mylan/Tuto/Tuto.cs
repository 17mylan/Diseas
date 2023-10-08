using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuto : MonoBehaviour
{
    public GameObject dashGameObjectWall;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Destroy(dashGameObjectWall);
        }
    }
}
