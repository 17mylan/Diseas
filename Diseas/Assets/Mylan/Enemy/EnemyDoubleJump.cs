using UnityEngine;
using UnityEngine.AI;

public class EnemyDoubleJump : MonoBehaviour
{
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float jumpHeight = 2.3f;
    public float jumpSpeed = 5f;
    public float attackCooldown = 1f;
    public NavMeshAgent _AI;
    public GameObject _playerReference;
    public Transform _exampleCharacter;
    public bool canAiMove = true;

    public float amplitude = 0.5f;
    public float frequency = 0f;
    public float speed = 1.2f;

    private float originalY;

    private enum EnemyState
    {
        Neutre,
        Chasse,
        Attaque
    }

    private EnemyState currentState;
    private float attackTimer;
    private Vector3 attackTargetPosition;

    private void Start()
    {
        _playerReference = GameObject.Find("ExampleCharacter");
        _exampleCharacter = _playerReference.transform;

        originalY = transform.position.y;

        currentState = EnemyState.Neutre;
        attackTimer = 0f;
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Neutre:
                UpdateNeutreState();
                break;
            case EnemyState.Chasse:
                UpdateChasseState();
                break;
            case EnemyState.Attaque:
                UpdateAttaqueState();
                break;
            default:
                break;
        }
    }

    private void UpdateNeutreState()
    {
        Debug.Log("Ennemi dans l'état Neutre");

        float distanceToPlayer = Vector3.Distance(transform.position, _exampleCharacter.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (distanceToPlayer <= attackRange)
            {
                currentState = EnemyState.Attaque;
                attackTimer = attackCooldown;
                attackTargetPosition = _exampleCharacter.position;
                Debug.Log("Joueur dans la plage d'attaque. Passage à l'état Attaque.");
            }
            else
            {
                currentState = EnemyState.Chasse;
                Debug.Log("Joueur dans la plage de détection mais pas dans la plage d'attaque. Passage à l'état Chasse.");
            }
            return;
        }

        // Logique pour l'état Neutre
        // ...
    }

    private void UpdateChasseState()
    {
        Debug.Log("Ennemi dans l'état Chasse");

        float distanceToPlayer = Vector3.Distance(transform.position, _exampleCharacter.position);

        if (distanceToPlayer > detectionRange)
        {
            currentState = EnemyState.Neutre;
            Debug.Log("Joueur hors de portée. Passage à l'état Neutre.");
            return;
        }

        if (distanceToPlayer <= attackRange)
        {
            currentState = EnemyState.Attaque;
            attackTimer = attackCooldown;
            attackTargetPosition = _exampleCharacter.position;
            Debug.Log("Joueur dans la plage d'attaque. Passage à l'état Attaque.");
            return;
        }

        Vector3 lookAtPlayer = new Vector3(_exampleCharacter.position.x, transform.position.y, _exampleCharacter.position.z);
        transform.LookAt(lookAtPlayer);

        float step = jumpSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _exampleCharacter.position, step);

        float pingPongValue = Mathf.PingPong(Time.time * speed, 1.0f);
        float sinusValue = Mathf.Sin(pingPongValue * Mathf.PI);
        float curvedValue = sinusValue * amplitude;

        float newY = originalY + curvedValue * jumpHeight;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            float groundHeight = hit.point.y + GetComponent<Collider>().bounds.extents.y;
            transform.position = new Vector3(transform.position.x, Mathf.Max(newY, groundHeight), transform.position.z);
        }
    }

    private void UpdateAttaqueState()
   {
        Debug.Log("Ennemi dans l'état Attaque");

        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
            // Pause avant l'attaque
            Debug.Log("Pause avant l'attaque : " + attackTimer.ToString("F2") + "s");
        }
        else
        {
            // Attaque
            JumpToAttackPosition();
        }
    }

    private void JumpToAttackPosition()
    {
        currentState = EnemyState.Neutre;
        Debug.Log("Attaque terminée. Passage à l'état Neutre.");
        // Implémentez la logique pour l'attaque (par exemple, sauter sur la position du joueur enregistrée)
        transform.position = attackTargetPosition;
    }
}
