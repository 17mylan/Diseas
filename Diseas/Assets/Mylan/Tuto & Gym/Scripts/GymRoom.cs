using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GymRoom : MonoBehaviour
{
    public Teleportation teleportation;
    public bool isReloadingGymRoom = false;
    public void Start()
    {
        teleportation = FindObjectOfType<Teleportation>();        
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.T) && !isReloadingGymRoom)
        {
            isReloadingGymRoom = true;
            teleportation.StartCoroutine(teleportation.TeleportPlayerWithDelay("TeleportationFromPart3toGymRoom", 1f));
        }
    }
}
