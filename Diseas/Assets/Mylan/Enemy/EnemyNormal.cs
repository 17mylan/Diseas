using UnityEngine;
using UnityEngine.AI;

public class EnemyNormal : MonoBehaviour
{
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
            float distanceToPlayer = Vector3.Distance(transform.position, _exampleCharacter.position);

            if (distanceToPlayer <= detectionRange)
            {
                _AI.destination = _exampleCharacter.position;
            }
            else
            {
                _AI.destination = transform.position;
            }
        }
    }
}
