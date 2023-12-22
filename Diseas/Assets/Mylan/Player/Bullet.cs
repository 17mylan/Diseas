using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float _WaitSecondsBulletToDestroy = 5f;
    public PlayerSoundsEffects playerSoundsEffects;
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Cible")
        {
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().SetEnemyStunned(true);
            playerSoundsEffects.playerSoundsEffectAudioSource.PlayOneShot(playerSoundsEffects.stunSound);
        }
        Destroy(gameObject);
    }
    public void Start()
    {
        Destroy(gameObject, _WaitSecondsBulletToDestroy);
        playerSoundsEffects = FindObjectOfType<PlayerSoundsEffects>();
    }
}
