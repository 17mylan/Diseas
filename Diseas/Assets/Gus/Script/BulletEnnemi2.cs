using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnnemi2 : MonoBehaviour
{
    public Transform spawnPoint, noEnemyTarget;
    public GameObject bulletPrefab, enemyTarget;
    public float bulletSpeed = 1f;

    void Start()
    {
        // Appeler la méthode CreateBullet avec l'ennemi toutes les 2 secondes
        InvokeRepeating("FireAtPlayer", 0f, 2f);
    }

    void FireAtPlayer()
    {
        if (spawnPoint != null && bulletPrefab != null && enemyTarget != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);
            Vector3 direction = (enemyTarget.transform.position - spawnPoint.position).normalized;
            bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
        }
    }

    void Update()
    {
        // Vous pouvez ajouter d'autres logiques ici si nécessaire
    }
}