using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;
using Cinemachine;

public class BallControlBehaviour : MonoBehaviour
{
    [Header("References - Cameras")]
    [SerializeField] private GameObject _vcPlayer;
    private CinemachineFreeLook _vcCinemachineCamera;
    
    // Toggle Input Control Camera
    private bool _islooking;
    void Start()
    {
        _islooking = false;
        _vcCinemachineCamera = _vcPlayer.GetComponent<CinemachineFreeLook>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) _islooking = !_islooking;
        if (_islooking)
        {
            //Set sur input Souris
            _vcCinemachineCamera.m_XAxis.m_InputAxisName = "Mouse X";
            _vcCinemachineCamera.m_YAxis.m_InputAxisName = "Mouse Y";
        }
        if (!_islooking)
        {
            //Unset sur input Souris
            _vcCinemachineCamera.m_XAxis.m_InputAxisName = "";
            _vcCinemachineCamera.m_YAxis.m_InputAxisName = "";
        }
    }
}
