using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class CompanionAI : MonoBehaviour
{
    public NavMeshAgent _AI;
    public Transform _playerReference, _linkToPlayer;
    public NavMeshLink _navMeshLink;
    Vector3 _dest;

    void Update()
    {

        _dest = _playerReference.position;
        _AI.destination = _dest;
        
        if(_AI.remainingDistance <= _AI.stoppingDistance)
        {
            //print("Companion do not move anymore.");
        }


        //Auto jump du player qui fonctionne a moitié
        //_linkToPlayer.transform.position = _playerReference.localPosition;
        //_navMeshLink.endPoint = _linkToPlayer.localPosition;   
        
        
    }
}
