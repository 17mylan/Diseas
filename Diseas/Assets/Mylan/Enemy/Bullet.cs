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
            Destroy(collision.gameObject);
            Destroy(gameObject);
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
