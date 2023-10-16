using UnityEngine;
using UnityEngine.AI;

public class EnemySuperpuissance : MonoBehaviour
{
    public float detectionRange = 10f; // Portée de détection
    public NavMeshAgent _AI;
    private Vector3 lastKnownPlayerPosition;
    private bool isMovingToPlayer = false;
    public GameObject _playerReference;
    public Transform _exampleCharacter;
    public bool canAiMove = true;

    public void Start()
    {
        _playerReference = GameObject.Find("ExampleCharacter");
        _exampleCharacter = _playerReference.transform;
    }
    void Update()
    {
        if(canAiMove)
        {
            if (!_AI.enabled)
            {
                _AI.enabled = true;
            }
            float distanceToPlayer = Vector3.Distance(transform.position, _exampleCharacter.position);

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
        else if(!canAiMove)
        {
            _AI.enabled = false;
        }
    }

    public void StartMovingToPlayer()
    {
        if (canAiMove)
        {
            // Le délai est écoulé, enregistrez la dernière position connue du joueur et commencez à vous déplacer vers lui
            lastKnownPlayerPosition = _exampleCharacter.position;
            _AI.destination = lastKnownPlayerPosition;

            // Définissez une autre fonction de rappel pour réinitialiser l'immobilité après le déplacement
            Invoke("StopMovingToPlayer", 1.5f);
        }
    }

    public void StopMovingToPlayer()
    {
        if(canAiMove)
        {
            // Réinitialisez la position de destination pour arrêter de se déplacer
            _AI.destination = transform.position;
            isMovingToPlayer = false;
        }
    }
}