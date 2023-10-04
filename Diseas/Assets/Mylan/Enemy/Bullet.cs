using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            if(_companion_Choice.ShootAndKill && !_companion_Choice.ShootAndStun)
                Destroy(collision.gameObject);
            else if(_companion_Choice.ShootAndStun && !_companion_Choice.ShootAndKill)
                collision.gameObject.GetComponent<Enemy>().SetEnemyStunned(true);

            Destroy(gameObject);
        }
    }
    public Companion_Choice _companion_Choice;
    public void Start()
    {
        _companion_Choice = FindObjectOfType<Companion_Choice>();
        StartCoroutine(WaitBulletAndDestroy());
    }
    public float _WaitSecondsBulletToDestroy = 1f;
    public IEnumerator WaitBulletAndDestroy()
    {
        yield return new WaitForSeconds(_WaitSecondsBulletToDestroy);
        Destroy(gameObject);
    }
}
