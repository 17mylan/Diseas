using UnityEngine;
using UnityEngine.AI;

public class Ennemi2 : MonoBehaviour
{
    [Range(1f, 50f)]
    public float detectionRange = 10f;
    public NavMeshAgent _AI;
    public Transform _playerReference;
    public BulletInstantiate bulletInstantiate; // Référence au script de gestion des balles

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _playerReference.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Le joueur est dans la plage de détection, déplacer l'ennemi vers le joueur
            _AI.destination = _playerReference.position;

            // Tirer sur le joueur
            bulletInstantiate.CreateBullet("WithEnemy");
        }
        else
        {
            // Le joueur est en dehors de la plage de détection, l'ennemi peut effectuer d'autres actions ici
            // Par exemple, arrêter de se déplacer
            _AI.destination = transform.position;
        }
    }
}