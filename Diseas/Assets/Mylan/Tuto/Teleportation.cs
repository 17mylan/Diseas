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
    public IEnumerator TeleportPlayerWithDelay(string _string, float _timer)
    {
        if(!isTeleporting)
        {
            isTeleporting = true;
            FadeToBlack.SetActive(true);
            if(_string == "Teleport1")
            {
                yield return new WaitForSeconds(_timer);
                Vector3 tpPosition = tutoPart2Position.position;
                Quaternion newRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                kinematicMotor.SetPositionAndRotation(tpPosition, newRotation, true);
                isTeleporting = false;
            }
            else if(_string == "Teleport2")
            {
                yield return new WaitForSeconds(_timer);
                Vector3 tpPosition = tutoPart3Position.position;
                Quaternion newRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                kinematicMotor.SetPositionAndRotation(tpPosition, newRotation, true);
                isTeleporting = false;
                yield return new WaitForSeconds(_timer);
                companionReference.transform.position = tpPosition;
            }
        }
    }
}
