using UnityEngine;
using UnityEngine.AI;

public class EnemyNormal : MonoBehaviour
{
    public float detectionRange = 10f; // Portée de détection
    public NavMeshAgent _AI;
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
                // Le joueur est dans la plage de détection, déplacer l'ennemi vers le joueur
                _AI.destination = _exampleCharacter.position;
            }
            else
            {
                // Le joueur est en dehors de la plage de détection
                _AI.destination = transform.position;
            }
        }
        else if (!canAiMove)
        {
            _AI.enabled = false;
        }
    }
}
