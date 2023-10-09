using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuto : MonoBehaviour
{
    public GameObject dashGameObjectWall, teleporterToPart2;
    public Transform teleportTransformPart2;
    public bool isTeleporting = false;
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Destroy(dashGameObjectWall);
        }
    }
    public IEnumerator SetTeleportingStatus()
    {
        isTeleporting = true;
        print(isTeleporting);
        yield return new WaitForSeconds(1f);
        isTeleporting = false;
        print(isTeleporting);
    }
}
