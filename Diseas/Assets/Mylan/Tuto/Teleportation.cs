using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportation : MonoBehaviour
{
    public Transform tutoPart2Position, tutoPart3Position;
    private KinematicCharacterController.KinematicCharacterMotor kinematicMotor;
    public GameObject FadeToBlack, FadeToWhite, companionReference;
    public FadeTransition fadeTransition;
    public bool isTeleporting = false;
    public void Start()
    {
        kinematicMotor = transform.GetComponent<KinematicCharacterController.KinematicCharacterMotor>();
    }
    public void TeleportPlayer()
    {
        StartCoroutine(TeleportPlayerWithDelay());
    }
    public void TeleportPlayer2()
    {
        StartCoroutine(TeleportPlayerWithDelay2());
    }
    public IEnumerator TeleportPlayerWithDelay()
    {
        if(!isTeleporting)
        {
            isTeleporting = true;
            FadeToBlack.SetActive(true);
            yield return new WaitForSeconds(1f);
            Vector3 tpPosition = tutoPart2Position.position;
            kinematicMotor.SetPosition(tpPosition, true);
            isTeleporting = false;
        }
    }
    public IEnumerator TeleportPlayerWithDelay2()
    {
        isTeleporting = true;
        FadeToBlack.SetActive(true);
        yield return new WaitForSeconds(1f);
        Vector3 tpPosition = tutoPart3Position.position;
        kinematicMotor.SetPosition(tpPosition, true);
        isTeleporting = false;
        yield return new WaitForSeconds(1f);
        companionReference.transform.position = tpPosition;
    }
}
