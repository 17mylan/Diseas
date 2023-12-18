using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour 
{
    public GameObject fractured;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
            BreakTheThing();
    }
    public void BreakTheThing() 
    {
      Instantiate(fractured, transform.position, transform.rotation);
      Destroy(fractured);
    }
}