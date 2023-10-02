using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool _isStun = false;
    public Material _originalMaterial, _stunMaterial;
    public GameObject _enemyReference;
    public float _waitTimerStunBeforeBack = 20f;

    public void SetEnemyStunned(bool _stunStatus)
    {
        
        _isStun = _stunStatus;
        
        if(_isStun)
        {
            this.GetComponent<Renderer>().material = _stunMaterial;
            StartCoroutine(WaitStunTimer());
        }
        else if(!_isStun)
        {
            this.GetComponent<Renderer>().material = _originalMaterial;
        }
    }

    public IEnumerator WaitStunTimer()
    {
        yield return new WaitForSeconds(_waitTimerStunBeforeBack);
        SetEnemyStunned(false);
    }
}
