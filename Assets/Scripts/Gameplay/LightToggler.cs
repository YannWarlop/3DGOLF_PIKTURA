using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightToggler : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private GameObject _light1;
    [SerializeField] private GameObject _light2;

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player")) {
            _light1.SetActive(true);
            _light2.SetActive(true);
        }
    }
}
