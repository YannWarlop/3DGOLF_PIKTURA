using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class CameraSwitchBehaviour : MonoBehaviour
{
    [FormerlySerializedAs("_cameraTarget")]
    [Header("References")]
    [SerializeField] private GameObject _roomTarget;
    [SerializeField] private GameObject _playerTarget;
    [SerializeField] private GameObject _cameraTarget;
    [Header("Camera Attributes")] 
    [SerializeField] private float _fov = 60f;
    [SerializeField] private float _nearPlane = 0.3f;
    [SerializeField] private float _farPlane = 1000f;
    [SerializeField] private float _orthographicSize = 8.5f;
    [Header("Room View Attributes")]
    [Header("Ball View Attributes")]
    
    //Necesaire pour le lerp Ortho - Persp
    private Matrix4x4 _orthographicMatrix;
    private Matrix4x4 _perspectiveMatrix;
    
    //Necesaire pour le controle du curseur
    private float mouseX;
    private float mouseY;
    
    private Vector3 _roomTargetCameraRotation; // Track la rotation desirée de la cam
    Vector3 _lastCameraRoomPosition; //Track de la position dans la room de la cam
    private bool _onRoomView; // Si vue Room ou vue Ball
    private Camera _camera; // Trivial
    private GameObject _cameraObject;

    void Start() {
        _cameraObject = this.gameObject;
        _roomTargetCameraRotation = _roomTarget.transform.rotation.eulerAngles;
        _camera = this.GetComponent<Camera>();
        // Setup des Matrices de Camera
        _orthographicMatrix = Matrix4x4.Ortho(-_orthographicSize * Screen.width / Screen.height,
            _orthographicSize * Screen.width / Screen.height, -_orthographicSize, _orthographicSize, _nearPlane,
            _farPlane);
        _perspectiveMatrix = Matrix4x4.Perspective(_fov, (float)16/9, _nearPlane, _farPlane);
        // Setup par défaut de la cam
        _camera.projectionMatrix = _orthographicMatrix;
        _onRoomView = true; // Par défaut vue room ig
    } 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_onRoomView) _lastCameraRoomPosition = _cameraObject.transform.position;
            Debug.Log(_lastCameraRoomPosition);
            CameraPerspectiveSwitch();
        }
        if (_onRoomView) {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) CameraRoomRotation(KeyCode.LeftArrow);
            if (Input.GetKeyDown(KeyCode.RightArrow)) CameraRoomRotation(KeyCode.RightArrow);
        }
        else {
            if (Input.GetMouseButton(1))
            {
                Cursor.lockState = CursorLockMode.Locked;
                CameraCursorControl();
            }
            if (Input.GetMouseButtonUp(1))
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
    void CameraCursorControl()
    {
        mouseX += Input.GetAxis("Mouse X");
        mouseY -= Input.GetAxis("Mouse Y");
        mouseY = Mathf.Clamp(mouseY, -90, 90);
        _cameraTarget.transform.rotation = Quaternion.Euler(_cameraTarget.transform.rotation.y + mouseY, _cameraTarget.transform.rotation.x + mouseX, 0);

    }
    
    void CameraRoomRotation(KeyCode key) {  
        if (key == KeyCode.LeftArrow) {
            _roomTargetCameraRotation += new Vector3(0,90,0);
            _cameraTarget.transform.DORotate(_roomTargetCameraRotation, 0.4f);
        }
        else if (key == KeyCode.RightArrow) {
            _roomTargetCameraRotation += new Vector3(0,-90,0);;
            _cameraTarget.transform.DORotate(_roomTargetCameraRotation, 0.4f);
        }
    }

    void CameraPerspectiveSwitch()
    {
        Debug.Log("Camera perspective switch");
        if (_onRoomView)
        {
            _onRoomView = false;
            //Reset de la rotation pour eviter un snapback
            BlendToMatrix(_perspectiveMatrix, 0.5f);
            _cameraTarget.transform.DOMove(_playerTarget.transform.position, 0.5f);
            _cameraObject.transform.DOMove(Vector3.MoveTowards(this.transform.position, _playerTarget.transform.position, Vector3.Distance(_cameraObject.transform.position, _playerTarget.transform.position) - 5f), 0.5f); // AVancement justa une distance de 5 + -1 en z
        }
        else if (!_onRoomView)
        {
            _onRoomView = true;
            BlendToMatrix(_orthographicMatrix, 0.5f);
            _cameraTarget.transform.DORotate(_roomTargetCameraRotation, 0.5f);
            _cameraTarget.transform.DOMove(_roomTarget.transform.position, 0.5f);
            _cameraObject.transform.DOMove(_lastCameraRoomPosition, 0.5f);

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
            _camera.projectionMatrix = MatrixLerp(src, dest, (Time.time - startTime) / duration); 
            yield return 1; 
        } 
        _camera.projectionMatrix = dest; 
    } 

    public Coroutine BlendToMatrix(Matrix4x4 targetMatrix, float duration) 
    { 
        StopAllCoroutines(); 
        return StartCoroutine(LerpFromTo(_camera.projectionMatrix, targetMatrix, duration)); 
    } 
}
