using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightToggler : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private List<GameObject> _lights;
    [SerializeField] private Animator _animator;
    
    [Header("References - Sounds")]
    [SerializeField] private AudioClip _toggleSound;

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player")) {
            SoundFXManager.Instance.PlaySoundFX(_toggleSound, 0.8f, gameObject.transform);
            foreach (GameObject _light  in _lights)
            {
                if (_light != null) _light.SetActive(true);
            }
            if (_animator != null) _animator.SetTrigger("Toggled");
        }
    }
}
