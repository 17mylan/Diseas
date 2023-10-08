using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadEnemyDetector : MonoBehaviour
{
    public float raycastDistance;
    public void Update()
    {
        Vector3 playerPosition = transform.position;
        Vector3 raycastDirection = Vector3.down;
        RaycastHit hit;
        if (Physics.Raycast(playerPosition, raycastDirection, out hit, raycastDistance))
        {
            // Le raycast a touché quelque chose
            Debug.DrawLine(playerPosition, hit.point, Color.red);
            if(hit.collider.tag == "EnemyHead")
            {
                Debug.Log("J'ai touché la tete d'un enemie et je l'ai tué");
                Destroy(hit.collider.transform.parent.gameObject);
            }
        }
        else
        {
            // Le raycast n'a rien touché
            Debug.DrawRay(playerPosition, raycastDirection * raycastDistance, Color.green);
        }
    }
}
