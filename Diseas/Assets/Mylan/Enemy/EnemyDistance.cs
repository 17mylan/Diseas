using UnityEngine;
using UnityEngine.AI;

public class EnemyDistance : MonoBehaviour
{
    public float desiredDistance = 5f; // Distance souhaitée
    public float detectionRange = 10f; // Portée de détection
    public float shootingInterval = 2f; // Intervalle entre les tirs
    public Transform bulletSpawnPoint; // Point d'apparition des balles
    public GameObject bulletEnemyPrefab; // Prefab de la balle ennemie
    public NavMeshAgent _AI;
    public Transform _playerReference;

    private float timeSinceLastShot = 0f;

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _playerReference.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Le joueur est dans la plage de détection
            Vector3 directionToPlayer = _playerReference.position - transform.position;
            Vector3 destination = _playerReference.position - directionToPlayer.normalized * desiredDistance;

            // Déplacer l'ennemi vers la destination tout en maintenant la distance souhaitée
            _AI.destination = destination;

            // Ajuster la rotation pour faire face au joueur
            transform.LookAt(_playerReference.position);

            // Tirer toutes les 2 secondes
            timeSinceLastShot += Time.deltaTime;
            if (timeSinceLastShot >= shootingInterval)
            {
                Shoot();
                timeSinceLastShot = 0f;
            }
        }
        else
        {
            // Le joueur est en dehors de la plage de détection
            _AI.destination = transform.position;
        }
    }

    void Shoot()
    {
        // Instancier la balle ennemie au point d'apparition
        Instantiate(bulletEnemyPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
    }
}
