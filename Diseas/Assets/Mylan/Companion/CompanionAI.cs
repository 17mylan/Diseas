using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class CompanionAI : MonoBehaviour
{
    public NavMeshAgent _AI;
    public Transform _playerReference, _nativeNavMeshLinkPoint, _linkToPlayer;
    public NavMeshLink _navMeshLink;
    Vector3 _dest;

    void Update()
    {

        _dest = _playerReference.position;
        _AI.destination = _dest;

        _linkToPlayer.position = _playerReference.position;
        _navMeshLink.endPoint = _linkToPlayer.localPosition;
        /*if(_AI.remainingDistance <= _AI.stoppingDistance)
        {
            print("Je suis pas loin de toi arrété.");
        }*/
    }
}
