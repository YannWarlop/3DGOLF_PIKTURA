using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;
using Cinemachine;

public class CameraGeneralBahviour : MonoBehaviour
{
    [Header("References - Cameras")]
    [SerializeField] private GameObject _vcRoom;
    [SerializeField] private GameObject _vcPlayer;
    private Camera _classicCamera;
    private CinemachineBrain _vcBrain;
    [Header("References - Objects")]
    [SerializeField] private GameObject _roomTarget;
    [SerializeField] private GameObject _playerTarget;
    [Header("Camera Attributes")] 
    [Header("Room View Attributes")]
    [Header("Ball View Attributes")]
    
    //Necesaire pour le lerp Ortho - Persp
    private Matrix4x4 _orthographicMatrix;
    private Matrix4x4 _perspectiveMatrix;
    
    public bool _onRoomView; // Si vue Room ou vue Ball
    public bool _onTransition; // Si la camera est en transition (Pour eviter les inputs pendant la transition
    private Vector3 _roomTargetRotation;
    private void Start()
    {
        //Setup Base Values
        _onTransition = false;
        _onRoomView = true; // temp pour debug
        _vcBrain = this.GetComponent<CinemachineBrain>();
        _classicCamera = this.GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_onTransition) CameraPerspectiveSwitch();
        if (_onRoomView)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && !_onTransition) CameraRoomRotation(KeyCode.LeftArrow);
            if (Input.GetKeyDown(KeyCode.RightArrow) && !_onTransition) CameraRoomRotation(KeyCode.RightArrow);
        }
    }
    private void CameraPerspectiveSwitch()
    {
        _onTransition = true; // On flag comme en transition -> Plus d'inputs autorisées
        if (_onRoomView) // Si vue de Room
        {
            _vcPlayer.GetComponent<CinemachineFreeLook>().Priority = 1; // Active la cam Player
            _vcRoom.GetComponent<CinemachineVirtualCamera>().Priority = 0; // Desactive la Cam Room
            //On Definis la matrice de la camera de destination
            CinemachineFreeLook _cmvCamera = _vcPlayer.GetComponent<CinemachineFreeLook>();
            _perspectiveMatrix = Matrix4x4.Perspective(_cmvCamera.m_Lens.FieldOfView, _cmvCamera.m_Lens.Aspect, _cmvCamera.m_Lens.NearClipPlane, _cmvCamera.m_Lens.FarClipPlane); 
            _onRoomView = !_onRoomView; // On passe en vue Player
            //ON DESACTIVE CINEMACHINE LE TEMPS DE FAIRE LA TRANSITION DE CAM CAR CINEMACHINE ETS INCAPABLE DE GERER UNE MATRICE DE PROJECTION
            _vcBrain.enabled = false; //Cinemachine OFF
            BlendToMatrix(_perspectiveMatrix, 1f); //ON BLEND LE FOV
            this.transform.DOMove(_vcPlayer.transform.position, 1f).SetEase(Ease.OutSine); //Camera Move
            _vcPlayer.transform.LookAt(_playerTarget.transform.position); // On rotate la cam pour une transition plus smooth
            //CINEMACHINE NE FAIT PAS ROTATE UNE CAMERA EN FREELOOK
            this.transform.DORotate(_vcPlayer.transform.eulerAngles, 1f).SetEase(Ease.OutSine); //CameraRotate
        }
        else if (!_onRoomView) //Si en Vue Player
        {
            //On Definis la matrice de la camera de destination
            CinemachineVirtualCamera _cmvCamera = _vcRoom.GetComponent<CinemachineVirtualCamera>();
            _orthographicMatrix = Matrix4x4.Ortho(-_cmvCamera.m_Lens.OrthographicSize * Screen.width / Screen.height, _cmvCamera.m_Lens.OrthographicSize * Screen.width / Screen.height, -_cmvCamera.m_Lens.OrthographicSize, _cmvCamera.m_Lens.OrthographicSize, _cmvCamera.m_Lens.NearClipPlane, _cmvCamera.m_Lens.FarClipPlane);
            _onRoomView = !_onRoomView; // On passe en vue Player
            _vcPlayer.GetComponent<CinemachineFreeLook>().Priority = 0; // Desactive la cam Player
            _vcRoom.GetComponent<CinemachineVirtualCamera>().Priority = 1; // Active la Cam Room
            //ON DESACTIVE CINEMACHINE LE TEMPS DE FAIRE LA TRANSITION DE CAM CAR CINEMACHINE ETS INCAPABLE DE GERER UNE MATRICE DE PROJECTION
            _vcBrain.enabled = false; //Cinemachine OFF
            BlendToMatrix(_orthographicMatrix, 1f); //ON BLEND LE FOV
            this.transform.DOMove(_vcRoom.transform.position, 1f).SetEase(Ease.OutSine); //Camera Move
            this.transform.DORotate(_vcRoom.transform.eulerAngles, 1f).SetEase(Ease.OutSine); //CameraRotate

        }
    }
    
    void CameraRoomRotation(KeyCode key) {  
        if (key == KeyCode.LeftArrow) {
            _roomTargetRotation += new Vector3(0,90,0);
            _roomTarget.transform.DORotate(_roomTargetRotation, 0.4f);
        }
        else if (key == KeyCode.RightArrow) {
            _roomTargetRotation += new Vector3(0,-90,0);;
            _roomTarget.transform.DORotate(_roomTargetRotation, 0.4f);
        }
    }
    
    // CAMERA PERSPECTIVE TO ORTHOGRAPHIC SWITCH FUNCTIONS
    public static Matrix4x4 MatrixLerp(Matrix4x4 from, Matrix4x4 to, float time) 
    { 
        Matrix4x4 ret = new Matrix4x4(); 
        for (int i = 0; i < 16; i++) 
            ret[i] = Mathf.Lerp(from[i], to[i], time); 
        return ret; 
    } 
    private IEnumerator LerpFromTo(Matrix4x4 src, Matrix4x4 dest, float duration) 
    { 
        float startTime = Time.time; 
        while (Time.time - startTime < duration) 
        { 
            _classicCamera.projectionMatrix = MatrixLerp(src, dest, (Time.time - startTime) / duration); 
            yield return 1; 
        } 
        _classicCamera.projectionMatrix = dest;
        _vcBrain.enabled = true; //Reactive Cinemachine
        _onTransition = false;
    } 

    public Coroutine BlendToMatrix(Matrix4x4 targetMatrix, float duration) 
    { 
        StopAllCoroutines(); 
        return StartCoroutine(LerpFromTo(_classicCamera.projectionMatrix, targetMatrix, duration)); 
    } 
}
