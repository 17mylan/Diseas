using UnityEngine;
using UnityEngine.AI;

public class EnemySuperpuissance : MonoBehaviour
{
    public float detectionRange = 10f; // Portée de détection
    public NavMeshAgent _AI;
    public Transform _playerReference;
    private Vector3 lastKnownPlayerPosition;
    private bool isMovingToPlayer = false;

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _playerReference.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (!isMovingToPlayer)
            {
                // Le joueur est dans la plage de détection, démarrez le délai avant de se déplacer
                Invoke("StartMovingToPlayer", 1.5f);
                isMovingToPlayer = true;
            }
        }
        else
        {
            // Le joueur est en dehors de la plage de détection, réinitialisez l'état si l'ennemi était en train de se déplacer
            if (isMovingToPlayer)
            {
                CancelInvoke("StartMovingToPlayer"); // Annulez le délai de déplacement si le joueur sort de la plage
                isMovingToPlayer = false;
            }
        }
    }

    void StartMovingToPlayer()
    {
        // Le délai est écoulé, enregistrez la dernière position connue du joueur et commencez à vous déplacer vers lui
        lastKnownPlayerPosition = _playerReference.position;
        _AI.destination = lastKnownPlayerPosition;

        // Définissez une autre fonction de rappel pour réinitialiser l'immobilité après le déplacement
        Invoke("StopMovingToPlayer", 1.5f);
    }

    void StopMovingToPlayer()
    {
        // Réinitialisez la position de destination pour arrêter de se déplacer
        _AI.destination = transform.position;
        isMovingToPlayer = false;
    }
}