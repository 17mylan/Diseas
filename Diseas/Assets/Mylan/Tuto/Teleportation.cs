using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportation : MonoBehaviour
{
    public Transform tutoPart2Position;
    private KinematicCharacterController.KinematicCharacterMotor kinematicMotor;
    public GameObject FadeToBlack, FadeToWhite;
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
}
