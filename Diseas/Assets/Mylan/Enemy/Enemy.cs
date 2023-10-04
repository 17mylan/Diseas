using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController.Examples;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool _isStun = false;
    public Material _originalMaterial, _stunMaterial;
    public GameObject _enemyReference;
    public float _waitTimerStunBeforeBack = 20f;

    public void SetEnemyStunned(bool _stunStatus)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        _isStun = _stunStatus;
        
        if(_isStun)
        {
            this.GetComponent<Renderer>().material = _stunMaterial;
            rb.isKinematic = true;
            StartCoroutine(WaitStunTimer());
        }
        else if(!_isStun)
        {
            this.GetComponent<Renderer>().material = _originalMaterial;
            rb.isKinematic = false;
        }
    }

    public IEnumerator WaitStunTimer()
    {
        yield return new WaitForSeconds(_waitTimerStunBeforeBack);
        SetEnemyStunned(false);
    }
    public BulletInstantiate _bulletInstantiate;
    public ExampleCharacterController _exampleCharacterController;
    public void Start()
    {
        _bulletInstantiate = FindObjectOfType<BulletInstantiate>();
        _exampleCharacterController = FindObjectOfType<ExampleCharacterController>();
    }
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if(_exampleCharacterController._isAiming)
                {
                    if (hit.collider == GetComponent<Collider>() && hit.collider.gameObject.tag == "Enemy")
                    {
                        _bulletInstantiate.enemyTarget = hit.collider.gameObject;
                        _bulletInstantiate.CreateBullet("WithEnemy");
                    }
                }
            }
        }
    }
}
