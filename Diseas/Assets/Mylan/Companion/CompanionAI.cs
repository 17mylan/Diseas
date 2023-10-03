using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class CompanionAI : MonoBehaviour
{
    public NavMeshAgent _AI;
    public Transform _playerReference;
    Vector3 _dest;
    void Update()
    {
        _dest = _playerReference.position;
        _AI.destination = _dest;

        /*if(_AI.remainingDistance <= _AI.stoppingDistance)
        {
            print("Je suis pas loin de toi arrété.");
        }*/
    }
}
