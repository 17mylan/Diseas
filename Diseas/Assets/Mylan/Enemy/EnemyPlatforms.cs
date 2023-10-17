using UnityEngine;
using UnityEngine.AI;

public class EnemyPlatforms : MonoBehaviour
{
    [Range(1f, 50f)]
    public float detectionRange = 10f;
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
                Vector3 directionToFlee = transform.position - _exampleCharacter.position;
                Vector3 destination = transform.position + directionToFlee.normalized * 10f;
                _AI.destination = destination;
            }
            else
            {
                _AI.destination = transform.position;
            }
        }
        else if (!canAiMove)
        {
            _AI.enabled = false;
        }
    }
}