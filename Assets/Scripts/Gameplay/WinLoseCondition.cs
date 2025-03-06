using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLoseCondition : MonoBehaviour
{
    //[Header("References")]
    public string WinLoseFlag = "";
    private Rigidbody _playerRb;
    private bool _isLit;
    private void Start() {
        //SETUP
        _playerRb = GetComponent<Rigidbody>();
    }
    private void Update() {
        if (_playerRb.velocity.magnitude < 0.05f && !_isLit)
            WinLoseFlag = "Lose";
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Finish") && _playerRb.velocity.magnitude < 1.5f)
            WinLoseFlag = "Win"; // Si enter basse vitesse dans Finish -> WinCon
        if (other.CompareTag("IsLit")) {
            RaycastHit _hit;
            // Calculate Ray direction
            Vector3 _direction = other.transform.position - _playerRb.transform.position;
            if (Physics.Raycast(transform.position, _direction, out _hit, _direction.magnitude)) { //On exclut les triggers pour n'avoir que les RB en hit
                _isLit = false;
            }
            else _isLit = true;
        }
    }

    private void OnTriggerExit(Collider other) { // Si exit de light Zone
        if (other.CompareTag("IsLit")) _isLit = false; //Flag le Bool
    }
}
