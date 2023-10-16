using UnityEngine;
using UnityEngine.AI;

public class EnemyPlatforms : MonoBehaviour
{
    [Range(1f, 50f)]
    public float detectionRange = 10f;
    public NavMeshAgent _AI;
    public Transform _playerReference;

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _playerReference.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Le joueur est dans la plage de détection, faire fuir l'ennemi
            Vector3 directionToFlee = transform.position - _playerReference.position;
            Vector3 destination = transform.position + directionToFlee.normalized * 10f; // 10f est la distance de fuite
            _AI.destination = destination;
        }
        else
        {
            // Le joueur est en dehors de la plage de détection, l'ennemi peut effectuer d'autres actions ici
            // Par exemple, arrêter de se déplacer
            _AI.destination = transform.position;
        }
    }
}