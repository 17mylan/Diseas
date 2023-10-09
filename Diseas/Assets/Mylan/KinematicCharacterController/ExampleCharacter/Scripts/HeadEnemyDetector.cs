using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadEnemyDetector : MonoBehaviour
{
    public float raycastDistance;
    public Tuto tuto;
    public void Update()
    {
        Vector3 playerPosition = transform.position;
        Vector3 raycastDirection = Vector3.down;
        RaycastHit hit;
        if (Physics.Raycast(playerPosition, raycastDirection, out hit, raycastDistance))
        {
            Debug.DrawLine(playerPosition, hit.point, Color.red);
            if(hit.collider.tag == "EnemyHead")
            {
                Debug.Log("J'ai touché la tete d'un enemie et je l'ai tué");
                Destroy(hit.collider.transform.parent.gameObject);
                if(tuto.isTutoEnabled)
                    tuto.TutoAddEnemyKilledToSaveHisCompanion(1);
            }
        }
    }
    public void Start()
    {
        tuto = FindObjectOfType<Tuto>();
    }
}
