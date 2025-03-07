using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{   
    public static SoundFXManager Instance; //Singleton Setup
    
    [Header("Sound FX")]
    [SerializeField] private AudioSource _audioSourcePrefab;

    private void Awake() {
        if (Instance == null) Instance = this;
    }

    public void PlaySoundFX(AudioClip _clip, float _volume, Transform _spawnPos) {
        AudioSource _newAudioSource = Instantiate(_audioSourcePrefab, _spawnPos.position, Quaternion.identity); //Spawn GameObject soundFX
        _newAudioSource.clip = _clip; //Assign AudioClip
        _newAudioSource.volume = _volume; //Set Volume
        _newAudioSource.Play(); //PlaySound
        float _clipLength = _clip.length;
        Destroy(_newAudioSource, _clipLength);
    }
}
