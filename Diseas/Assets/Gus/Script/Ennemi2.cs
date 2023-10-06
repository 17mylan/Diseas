using UnityEngine;
using UnityEngine.AI;

public class Ennemi2 : MonoBehaviour
{
    public float detectionRange = 10f; // Portée de détection
    public float attackRange = 5f; // Portée d'attaque
    public float fireRate = 2f; // Taux de tir en secondes
    private float nextFireTime = 0f;
    public NavMeshAgent _AI;
    public Transform _playerReference;
    public GameObject projectilePrefab; // Remplacez cela par le prefab de votre sphère 3D
    public Transform projectileSpawnPoint;

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _playerReference.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Le joueur est dans la plage de détection
            if (distanceToPlayer <= attackRange && Time.time >= nextFireTime)
            {
                // Le joueur est également dans la plage d'attaque, tirer une sphère 3D
                ShootProjectile();
                nextFireTime = Time.time + 1f / fireRate; // Met à jour le prochain temps de tir
            }
            else
            {
                // Le joueur est dans la plage de détection mais pas dans la plage d'attaque, déplacer l'ennemi vers le joueur
                _AI.destination = _playerReference.position;
            }
        }
        else
        {
            // Le joueur est en dehors de la plage de détection, l'ennemi peut effectuer d'autres actions ici
            // Par exemple, arrêter de se déplacer
            _AI.destination = transform.position;
        }
    }

    void ShootProjectile()
    {
        // Vous devrez remplacer le "projectilePrefab" par le prefab réel que vous utilisez pour votre sphère 3D
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

        // Calculer la direction vers le joueur
        Vector3 directionToPlayer = (_playerReference.position - projectileSpawnPoint.position).normalized;

        // Ajoutez ici le code pour déplacer le projectile (par exemple, rigidbody.velocity pour un Rigidbody)
        projectile.GetComponent<Rigidbody>().velocity = directionToPlayer * 20f; // Remplacez 10f par la vitesse souhaitée du projectile

        // Assurez-vous de détruire le projectile après un certain temps ou lorsqu'il entre en collision avec quelque chose
        Destroy(projectile, 5f); // Remplacez 5f par la durée de vie souhaitée du projectile
    }
}