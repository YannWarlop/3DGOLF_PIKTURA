using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZone_PushPull : MonoBehaviour
{
    private void OnTriggerStay(Collider _other) {
        Debug.Log("Trigger");
        Debug.Log(_other.gameObject.name);
        if (_other.gameObject.CompareTag("Player")) {
            _other.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * 15);
            Debug.Log("Force");
        }
    }
}
