using UnityEngine;
using UnityEngine.AI;

public class EnemyDoubleJump : MonoBehaviour
{
    public float detectionRange = 10f;
    public float jumpHeight = 2.3f;
    public float jumpSpeed = 6.5f;
    public NavMeshAgent _AI;
    public GameObject _playerReference;
    public Transform _exampleCharacter;
    public bool canAiMove = true;

    public float amplitude = 1.0f;
    public float frequency = 1.0f;
    public float speed = 1.0f;

    private float originalY;

    public void Start()
    {
        _playerReference = GameObject.Find("ExampleCharacter");
        _exampleCharacter = _playerReference.transform;

        originalY = transform.position.y;
    }

    void Update()
    {
        if (canAiMove && _AI.speed > 0)
        {
            if (!_AI.enabled)
            {
                _AI.enabled = true;
            }

            float distanceToPlayer = Vector3.Distance(transform.position, _exampleCharacter.position);

            if (distanceToPlayer <= detectionRange)
            {
                // Rotation de l'ennemi pour faire face au joueur
                Vector3 lookAtPlayer = new Vector3(_exampleCharacter.position.x, transform.position.y, _exampleCharacter.position.z);
                transform.LookAt(lookAtPlayer);

                // Déplacement du cube vers le joueur avec des petits sauts
                float step = jumpSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, _exampleCharacter.position, step);

                // Faites en sorte que le cube saute avec une courbe plus arrondie
                float pingPongValue = Mathf.PingPong(Time.time * speed, 1.0f);
                float sinusValue = Mathf.Sin(pingPongValue * Mathf.PI);
                float curvedValue = sinusValue * amplitude;

                float newY = originalY + curvedValue * jumpHeight;

                // Correction de la position verticale pour éviter l'enfoncement dans le sol
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
                {
                    float groundHeight = hit.point.y + GetComponent<Collider>().bounds.extents.y;
                    transform.position = new Vector3(transform.position.x, Mathf.Max(newY, groundHeight), transform.position.z);
                }
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
