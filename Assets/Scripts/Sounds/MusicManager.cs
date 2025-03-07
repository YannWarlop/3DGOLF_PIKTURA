using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance; //Singleton Setup
    
    [Header("References")]
    [SerializeField] private AudioSource _audioSourcePrefab;
    
    [Header("MusicList")]
    [SerializeField] private AudioClip _music1;
    [SerializeField] private AudioClip _music2;
    [SerializeField] private AudioClip _music3;
    [SerializeField] private AudioClip _music4;
    [SerializeField] private AudioClip _music5;
    [SerializeField] private AudioClip _music6;
    private void Awake() {
        if (Instance == null) Instance = this;
        
        //TEMP, a rendre selectionable
        PlayMusic(_music1, 0.8f, gameObject.transform);
    }

    public void PlayMusic(AudioClip _clip, float _volume, Transform _spawnPos) {
        AudioSource _newAudioSource = Instantiate(_audioSourcePrefab, _spawnPos.position, Quaternion.identity); //Spawn GameObject soundFX
        _newAudioSource.clip = _clip; //Assign AudioClip
        _newAudioSource.volume = _volume; //Set Volume
        _newAudioSource.Play(); //PlaySound
        _newAudioSource.loop = true; //LoopingMusic
        
       
    }
}
