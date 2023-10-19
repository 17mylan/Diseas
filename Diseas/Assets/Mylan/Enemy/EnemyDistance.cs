using UnityEngine;
using UnityEngine.AI;

public class EnemyDistance : MonoBehaviour
{
    public float desiredDistance = 5f;
    public float detectionRange = 10f;
    public float shootingInterval = 2f;
    public Transform bulletSpawnPoint;
    public GameObject bulletEnemyPrefab;
    public NavMeshAgent _AI;
    public GameObject _playerReference;
    private float timeSinceLastShot = 0f;
    public Transform _exampleCharacter;
    public bool canAiMove = true;

    public void Start()
    {
        _playerReference = GameObject.Find("ExampleCharacter");
        _exampleCharacter = _playerReference.transform;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _exampleCharacter.position);

        if (distanceToPlayer <= detectionRange)
        {
            Vector3 directionToPlayer = _exampleCharacter.position - transform.position;
            Vector3 destination = _exampleCharacter.position - directionToPlayer.normalized * desiredDistance;
            _AI.destination = destination;
            transform.LookAt(_exampleCharacter.position + new Vector3(0, 1, 0));
            timeSinceLastShot += Time.deltaTime;
            if (timeSinceLastShot >= shootingInterval)
            {
                Shoot();
                timeSinceLastShot = 0f;
            }
        }
        else
        {
            _AI.destination = transform.position;
        }
    }

    void Shoot()
    {
        if(_AI.speed > 0)
            Instantiate(bulletEnemyPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
    }
}
