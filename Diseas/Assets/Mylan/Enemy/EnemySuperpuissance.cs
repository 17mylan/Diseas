using UnityEngine;
using UnityEngine.AI;

public class EnemySuperpuissance : MonoBehaviour
{
    public float detectionRange = 10f;
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
                    Invoke("StartMovingToPlayer", 1.5f);
                    isMovingToPlayer = true;
                }
            }
            else
            {
                if (isMovingToPlayer)
                {
                    CancelInvoke("StartMovingToPlayer");
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
            lastKnownPlayerPosition = _exampleCharacter.position;
            _AI.destination = lastKnownPlayerPosition;
            Invoke("StopMovingToPlayer", 1.5f);
        }
    }

    public void StopMovingToPlayer()
    {
        if(canAiMove)
        {
            _AI.destination = transform.position;
            isMovingToPlayer = false;
        }
    }
}