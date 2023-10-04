using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInstantiate : MonoBehaviour
{
    public Transform spawnPoint, noEnemyTarget;
    public GameObject bulletPrefab, enemyTarget;
    public float bulletSpeed = 1f;
    public void CreateBullet(string _string)
    {
        if (spawnPoint != null && bulletPrefab != null)
        {
            if(_string == "WithEnemy")
            {
                if (spawnPoint != null && bulletPrefab != null)
                {
                    GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);
                    Vector3 direction = (enemyTarget.transform.position - spawnPoint.position).normalized;
                    bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;   
                }            
            }
            else if (_string == "WithoutEnemy")
            {
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    Vector3 targetPoint = ray.GetPoint(10f); // Choisir distance
                    GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);
                    Vector3 direction = (targetPoint - spawnPoint.position).normalized;
                    bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
                }
            }
        }
    }
}
