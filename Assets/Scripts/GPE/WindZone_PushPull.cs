using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZone_PushPull : MonoBehaviour
{
    [SerializeField] public float _speedMult = 1f;

    private void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, 50f * (_speedMult * 10) * Time.deltaTime); 
    }
    
    private void OnTriggerStay(Collider _other) {
        Debug.Log("Trigger");
        Debug.Log(_other.gameObject.name);
        if (_other.gameObject.CompareTag("Player")) {
            _other.gameObject.GetComponent<Rigidbody>().AddForce((transform.up * 15) * _speedMult);
            Debug.Log("Force");
        }
    }
}
