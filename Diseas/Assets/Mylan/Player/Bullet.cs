using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float _WaitSecondsBulletToDestroy = 5f;
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Cible")
        {
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().SetEnemyStunned(true);
        }
        Destroy(gameObject);
    }
    public void Start()
    {
        Destroy(gameObject, _WaitSecondsBulletToDestroy);
    }
}
