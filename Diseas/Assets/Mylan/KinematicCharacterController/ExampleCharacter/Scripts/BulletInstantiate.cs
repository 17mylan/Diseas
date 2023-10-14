using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInstantiate : MonoBehaviour
{
    public Transform spawnPoint, noEnemyTarget;
    public GameObject bulletPrefab, enemyTarget;
    public float bulletSpeed = 1f;
}
