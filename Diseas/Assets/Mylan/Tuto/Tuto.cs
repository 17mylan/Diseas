using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Tuto : MonoBehaviour
{
    public bool isTutoEnabled = true;
    public GameObject dashGameObjectWall, teleporterToPart2, teleporterToPart3;
    public Transform teleportTransformPart2, teleportTransformPart3;
    public bool isTeleporting = false;
    public bool hasKilledAllEnemyAndSavedHisCompanion = false;
    public int numberOfEnemyKilled = 0;
    public int maxNumberOfEnemyKilled = 8;
    public GameObject companionReference;
    public CompanionAI companionAI;
    public GameObject wallAfterSavecYourCompanion;
    public void Start()
    {
        companionAI = FindObjectOfType<CompanionAI>();
    }
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
        yield return new WaitForSeconds(1f);
        isTeleporting = false;
    }
    public void TutoAddEnemyKilledToSaveHisCompanion(int _number)
    {
        numberOfEnemyKilled = numberOfEnemyKilled + _number;
        if(numberOfEnemyKilled >= maxNumberOfEnemyKilled)
        {
            print("J'ai tué tout les enemies et j'ai libéré mon poto");
            companionAI.isCompanionFree = true;
            StartCoroutine(Dialogue());
        }
    }
    public IEnumerator Dialogue()
    {
        yield return new WaitForSeconds(2f);
        Destroy(wallAfterSavecYourCompanion);
    }
}
