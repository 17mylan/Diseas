using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class CompanionAI : MonoBehaviour
{
    public bool CanFollowPlayerAnywhere = true;
    public NavMeshAgent _AI;
    public Transform _playerReference, _linkToPlayer;
    public NavMeshLink _navMeshLink;
    Vector3 _dest;
    public Tuto tuto;
    public bool isCompanionFree = false;
    public void Start()
    {
        tuto = FindObjectOfType<Tuto>();
    }

    void Update()
    {
        if(isCompanionFree)
        {
            _dest = _playerReference.position;
            _AI.destination = _dest;

            // Auto Jump de partout ou il y a du NavMeshAgent
            if(CanFollowPlayerAnywhere)
            {
                if(!_navMeshLink.enabled)
                    _navMeshLink.enabled = true;

                _linkToPlayer.transform.position = _playerReference.localPosition;
                _navMeshLink.endPoint = _linkToPlayer.localPosition / 2;   
            }
            else if(!CanFollowPlayerAnywhere)
            {
                _navMeshLink.enabled = false;
            }
            if(_AI.remainingDistance <= _AI.stoppingDistance)
            {
                //print("Companion do not move anymore.");
            }
        }
    }
}
