using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController.Examples;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;

public class Tuto : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject dashGameObjectWall;
    public GameObject teleporterToPart2;
    public GameObject teleporterToPart3;
    public GameObject wallDash;
    public GameObject companionReference;
    public GameObject wallAfterSavecYourCompanion;
    public GameObject dialogueTextObject;
    public BoxCollider colliderDetectorToDash;
    [Header("Boolean")]
    public bool isTutoEnabled = true;
    public bool isTeleporting = false;
    public bool hasKilledAllEnemyAndSavedHisCompanion = false;
    public bool hasPassedColliderDetectorToDestroyWallForDash = false;
    public bool isInPhaseToJumpInHeadOfEnemies = false;
    [Header("Other")]
    public TextMeshProUGUI dialogueText;
    [TextArea]
    public string dialogue1, dialogue2;
    public Transform teleportTransformPart2;
    public Transform teleportTransformPart3;
    public int numberOfEnemyKilled = 0;
    public int maxNumberOfEnemyKilled = 8;
    public CompanionAI companionAI;
    public void Start()
    {
        companionAI = FindObjectOfType<CompanionAI>();
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
            StartCoroutine(Dialogue());
        }
    }
    public IEnumerator Dialogue()
    {
        yield return StartCoroutine(TypeSentence(dialogue1, 0.05f));
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(TypeSentence(dialogue2, 0.05f));
        yield return new WaitForSeconds(3f);
        companionAI.isCompanionFree = true;
        Destroy(wallAfterSavecYourCompanion);
        dialogueTextObject.SetActive(false);
    }
    public void DestroyWallWhenPlayerDashedOnTutoEnabled()
    {
        Destroy(wallDash);
    }
    public IEnumerator TypeSentence(string sentence, float typingSpeed)
    {
        dialogueTextObject.SetActive(true);
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
