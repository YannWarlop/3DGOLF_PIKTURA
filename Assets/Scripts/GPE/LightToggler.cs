using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LightToggler : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private List<GameObject> _lights;
    [SerializeField] private Animator _animator;
    [SerializeField] private WindZone_PushPull _windZone;
    
    [Header("References - Sounds")]
    [SerializeField] private AudioClip _toggleSound;

    private void Awake()
    {
        //BRUTEFORCE SALE
        if (SceneManager.GetActiveScene().name == "LV5") _animator.SetTrigger("ToggledLV5");
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player")) {
            SoundFXManager.Instance.PlaySoundFX(_toggleSound, 0.8f, gameObject.transform);
            foreach (GameObject _light  in _lights)
            {
                if (_light != null) _light.SetActive(true);
            }

            if (_animator != null) {
                _animator.SetTrigger($"Toggled{SceneManager.GetActiveScene().name}");
                
            }
            if (_windZone != null) _windZone._speedMult = 2.5f;
        }
    }
}
