using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    private void OnCollisionEnter(Collision _col) {
        if (_col.gameObject.CompareTag("Player")) _col.gameObject.transform.parent = gameObject.transform;
    }
    private void OnCollisionExit(Collision _col) {
        if (_col.gameObject.CompareTag("Player")) _col.gameObject.transform.parent = null;
    }
}
