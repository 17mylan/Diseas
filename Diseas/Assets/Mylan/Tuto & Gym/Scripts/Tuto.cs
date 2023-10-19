using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController.Examples;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tuto : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject dashGameObjectWall;
    public GameObject teleporterToPart2;
    public GameObject teleporterToPart3;
    public GameObject teleportToGymRoom;
    public GameObject wallDash;
    public GameObject companionReference;
    public GameObject wallAfterSavecYourCompanion;
    public GameObject dialogueTextObject;
    public BoxCollider colliderDetectorToDash;
    public GameObject dialogueBackground;
    [Header("Boolean")]
    public bool isTutoEnabled = true;
    public bool isTeleporting = false;
    public bool hasKilledAllEnemyAndSavedHisCompanion = false;
    public bool hasPassedColliderDetectorToDestroyWallForDash = false;
    public bool isInPhaseToJumpInHeadOfEnemies = false;
    [Header("Other")]
    public TextMeshProUGUI dialogueText;
    public GameObject imageDialogueReference;
    public Image dialogueImage;
    public Sprite dialoguePlayer;
    public Sprite compagnonDialogue;
    public float typingSpeed = 0.03f;
    public float waitTimerWhenDialogueIsFinished = 3f;
    [TextArea]
    public string dialogue1, dialogue2, dialogue3, dialogue4;
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
        dialogueBackground.SetActive(true);
        imageDialogueReference.SetActive(true);
        dialogueImage.GetComponent<Image>().sprite = compagnonDialogue;
        yield return StartCoroutine(TypeSentence(dialogue1, typingSpeed));
        dialogueImage.GetComponent<Image>().sprite = dialoguePlayer;
        yield return StartCoroutine(TypeSentence(dialogue2, typingSpeed));
        dialogueImage.GetComponent<Image>().sprite = compagnonDialogue;
        yield return StartCoroutine(TypeSentence(dialogue3, typingSpeed));
        dialogueImage.GetComponent<Image>().sprite = dialoguePlayer;
        yield return StartCoroutine(TypeSentence(dialogue4, typingSpeed));
        Destroy(wallAfterSavecYourCompanion);
        dialogueBackground.SetActive(false);
        companionAI.isCompanionFree = true;
        dialogueTextObject.SetActive(false);
        imageDialogueReference.SetActive(false);
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
        yield return new WaitForSeconds(waitTimerWhenDialogueIsFinished);
    }
}
