using UnityEngine;
using UnityEngine.AI;

public class EnemyDoubleJump : MonoBehaviour
{
    public float detectionRange = 10f;
    public float jumpHeight = 2f; // Hauteur des sauts
    public float jumpSpeed = 5f; // Vitesse des sauts
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
        if (canAiMove)
        {
            if (!_AI.enabled)
            {
                _AI.enabled = true;
            }

            float distanceToPlayer = Vector3.Distance(transform.position, _exampleCharacter.position);

            if (distanceToPlayer <= detectionRange)
            {
                // Déplacement du cube vers le joueur avec des petits sauts
                Vector3 targetPosition = new Vector3(_exampleCharacter.position.x, _exampleCharacter.position.y, _exampleCharacter.position.z);
                float step = jumpSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

                // Faites en sorte que le cube saute légèrement pendant le déplacement
                float newY = Mathf.PingPong(Time.time * jumpSpeed, jumpHeight);
                transform.position = new Vector3(transform.position.x, newY, transform.position.z);
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