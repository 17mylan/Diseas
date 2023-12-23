using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BiomeIntrocution : MonoBehaviour
{
    public PlayerSoundsEffects playerSoundsEffects;
    public GameObject WallAfterBiomeIntroduction, WallAfterBiomeIntroduction2, dialogueTextObject, imageDialogueReference, dialogueBackground;
    public Sprite compagnonDialogue;
    public Image dialogueImage;
    public float typingSpeed = 0.03f, waitingTime = 4f;
    public TextMeshProUGUI dialogueText;
    [TextArea]
    public string dialogue1, dialogue2, dialogue3, dialogue4, dialogue5, dialogue6, dialogue7;

    public void Start()
    {
        playerSoundsEffects = FindObjectOfType<PlayerSoundsEffects>();
        StartCoroutine(Dialogue());
    }

    public IEnumerator Dialogue()
    {
        dialogueBackground.SetActive(true);
        imageDialogueReference.SetActive(true);

        dialogueImage.GetComponent<Image>().sprite = compagnonDialogue;
        yield return StartCoroutine(TypeSentence(dialogue1, typingSpeed));

        yield return StartCoroutine(TypeSentence(dialogue2, typingSpeed));
        yield return StartCoroutine(TypeSentence(dialogue3, typingSpeed));

        yield return StartCoroutine(TypeSentence(dialogue4, typingSpeed));

        yield return StartCoroutine(TypeSentence(dialogue5, typingSpeed));

        waitingTime = 6f;
        yield return StartCoroutine(TypeSentence(dialogue6, typingSpeed));

        waitingTime = waitingTime;
        yield return StartCoroutine(TypeSentence(dialogue7, typingSpeed));

        Destroy(WallAfterBiomeIntroduction);
        Destroy(WallAfterBiomeIntroduction2);
        dialogueBackground.SetActive(false);
        dialogueTextObject.SetActive(false);
        imageDialogueReference.SetActive(false);
        playerSoundsEffects.playerSoundsEffectAudioSource.PlayOneShot(playerSoundsEffects.destroySound);
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
        yield return new WaitForSeconds(waitingTime);
    }
}
