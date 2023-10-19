using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Teleportation : MonoBehaviour
{
    public Transform tutoPart2Position, tutoPart3Position;
    private KinematicCharacterController.KinematicCharacterMotor kinematicMotor;
    public Tuto tuto;
    public GameObject FadeToBlack, FadeToWhite, companionReference;
    public FadeTransition fadeTransition;
    public bool isTeleporting = false;
    public void Start()
    {
        tuto = FindObjectOfType<Tuto>();
        kinematicMotor = transform.GetComponent<KinematicCharacterController.KinematicCharacterMotor>();
    }
    public IEnumerator TeleportPlayerWithDelay(string _string, float _timer)
    {
        if(!isTeleporting)
        {
            isTeleporting = true;
            FadeToBlack.SetActive(true);
            if(_string == "TeleportationFromPart1toPart2")
            {
                yield return new WaitForSeconds(_timer);
                Vector3 tpPosition = tutoPart2Position.position;
                Quaternion newRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                kinematicMotor.SetPositionAndRotation(tpPosition, newRotation, true);
                isTeleporting = false;
                tuto.isInPhaseToJumpInHeadOfEnemies = true;
            }
            else if(_string == "TeleportationFromPart2toPart3")
            {
                tuto.isInPhaseToJumpInHeadOfEnemies = false;
                yield return new WaitForSeconds(_timer);
                Vector3 tpPosition = tutoPart3Position.position;
                Quaternion newRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                kinematicMotor.SetPositionAndRotation(tpPosition, newRotation, true);
                Vector3 tpPositionCompanion = tutoPart3Position.position;
                tpPositionCompanion.x -= 1f;
                companionReference.SetActive(false);
                companionReference.transform.position = tpPositionCompanion;
                companionReference.SetActive(true);
                isTeleporting = false;
                yield return new WaitForSeconds(_timer);
                companionReference.transform.position = tpPosition;
            }
            else if(_string == "TeleportationFromPart3toGymRoom")
            {
                yield return new WaitForSeconds(_timer);
                SceneManager.LoadScene("GymRoom");
            }
            else if(_string == "TeleportationFromGymRoomToBiome")
            {
                yield return new WaitForSeconds(_timer);
                SceneManager.LoadScene("Menu");
            }
        }
    }
}
