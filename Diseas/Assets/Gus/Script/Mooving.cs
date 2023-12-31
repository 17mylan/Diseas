using UnityEngine;
using UnityEngine.AI;

public class Mooving : MonoBehaviour
{
    public float detectionRange = 10f; // Portée de détection
    public NavMeshAgent _AI;
    public Transform _playerReference;

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _playerReference.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Le joueur est dans la plage de détection, déplacer l'ennemi vers le joueur
            _AI.destination = _playerReference.position;
        }
        else
        {
            // Le joueur est en dehors de la plage de détection, l'ennemi peut effectuer d'autres actions ici
            // Par exemple, arrêter de se déplacer
            _AI.destination = transform.position;
        }
    }
}