using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        if(collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().SetEnemyStunned(true);
        }
    }
    public void Start()
    {
        StartCoroutine(WaitBulletAndDestroy());
    }
    public float _WaitSecondsBulletToDestroy = 1f;
    public IEnumerator WaitBulletAndDestroy()
    {
        yield return new WaitForSeconds(_WaitSecondsBulletToDestroy);
        Destroy(gameObject);
    }
}
