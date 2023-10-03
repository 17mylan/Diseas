using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInstantiate : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject bulletPrefab, enemyTarget;
    public float bulletSpeed = 1f;
    public void CreateBullet()
    {
        if (spawnPoint != null && bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);
            Vector3 direction = (enemyTarget.transform.position - spawnPoint.position).normalized;
            bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;   
        }            
    }
}
