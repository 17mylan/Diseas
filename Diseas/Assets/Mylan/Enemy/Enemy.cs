using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public bool _isStun = false;
    public Material _originalMaterial, _stunMaterial;
    public GameObject _enemyReference;
    public float _waitTimerStunBeforeBack = 20f;
    public ExampleCharacterController exampleCharacterController;
    public GameObject lifePrefab;
    public float navMeshSpeed;

    public void SetEnemyStunned(bool _stunStatus)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        _isStun = _stunStatus;
        
        if(_isStun)
        {
            this.GetComponent<Renderer>().material = _stunMaterial;
            navMeshSpeed = this.GetComponent<NavMeshAgent>().speed;
            this.GetComponent<NavMeshAgent>().speed = 0f;
            rb.isKinematic = true;
            StartCoroutine(WaitStunTimer());
        }
        else if(!_isStun)
        {
            this.GetComponent<Renderer>().material = _originalMaterial;
            this.GetComponent<NavMeshAgent>().speed = navMeshSpeed;
            rb.isKinematic = false;
        }
    }
    public void OnCollisionEnter(Collision hitCollider)
    {
        if(hitCollider.gameObject.tag == "Player" && _exampleCharacterController._isDashing && _exampleCharacterController._hasSuperpuissanceCapacity)
        {
            if (this.GetComponent<EnemyPlatforms>() != null)
                _exampleCharacterController.GiveCapacityToPlayer("Platforming");
            else if (this.GetComponent<EnemySuperpuissance>() != null)
                _exampleCharacterController.GiveCapacityToPlayer("Superpuissance");
            else if (this.GetComponent<EnemyDoubleJump>() != null)
                _exampleCharacterController.GiveCapacityToPlayer("DoubleJump");
                GameObject vie = Instantiate(lifePrefab, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
    public IEnumerator WaitStunTimer()
    {
        yield return new WaitForSeconds(_waitTimerStunBeforeBack);
        SetEnemyStunned(false);
    }
    public ExampleCharacterController _exampleCharacterController;
    public void Start()
    {
        _exampleCharacterController = FindObjectOfType<ExampleCharacterController>();
    }
}
