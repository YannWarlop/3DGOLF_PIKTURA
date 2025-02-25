using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;
using Cinemachine;

public class BallControlBehaviour : MonoBehaviour
{
    TrajectoryPredictor trajectoryPredictor;
    
    [Header("References - GameObjects")]
    [SerializeField] private GameObject _vcPlayer;
    [SerializeField] private LineRenderer _lineTrajectory;
    [SerializeField] private GameObject _cameratargetPlayer;
    [SerializeField] CameraGeneralBahviour cameraGeneralBahviour; //Infos de CameraState Necessaire
    private CinemachineFreeLook _vcCinemachineCamera;
    private Rigidbody _ballRigidbody;
    
    // Toggle Input Control Camera
    private bool _isViewing;
    private bool _islooking;
    private bool _isMouseAiming;

    // Shoot Related
    private Vector3 _predictedDirectionXYZ;
    private float _shootForce = 0f;
    void Start()
    {
        //Setup
        _isViewing = false;
        _islooking = false;
        _isMouseAiming = false;
        _predictedDirectionXYZ = Vector3.forward;
        //Components
        _vcCinemachineCamera = _vcPlayer.GetComponent<CinemachineFreeLook>();
        trajectoryPredictor = GetComponent<TrajectoryPredictor>();
        _ballRigidbody = GetComponent<Rigidbody>();
    }
    
    
    void Update()
    {
        _cameratargetPlayer.transform.position = this.transform.position; // On aligne la cible camera sur la balle
        if (cameraGeneralBahviour._onRoomView == false && cameraGeneralBahviour._onTransition == false) // Si on est bien sur la POV Player
        {
            if (Input.GetMouseButtonDown(1))
            {
                _islooking = !_islooking;
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
            if (Input.GetMouseButtonDown(0)) _isMouseAiming = !_isMouseAiming; //Toggle Mouse Aim
            
            if (_ballRigidbody.velocity.magnitude < 0.01f) //SSI BALLE IMMOBILE - Predicition + Shoot;
            {
                trajectoryPredictor.enabled = true;
                
                //Trajectory Prevew
                BallProperties _properties() { //Data Fetch
                    BallProperties ballProperties = new BallProperties();
                    Rigidbody r = _ballRigidbody;
                    ballProperties.direction = _predictedDirectionXYZ;
                    ballProperties.initialPosition = this.transform.position;
                    ballProperties.initialForce = _shootForce;
                    ballProperties.mass = _ballRigidbody.mass;
                    ballProperties.drag = _ballRigidbody.drag;
                    return ballProperties; }
                trajectoryPredictor.PredictTrajectory(_properties()); //Predict Start
                //Si mouseaim vector XZ par camera
                if (_isMouseAiming) _predictedDirectionXYZ = new Vector3(this.transform.position.x - _vcPlayer.transform.position.x, 0, this.transform.position.z - _vcPlayer.transform.position.z).normalized;
                else ManualAim();
            } 
            else {
                trajectoryPredictor.enabled = false; //Si mouvement , no preview
                ResetProperties(); //Reset Values
            }
        }
        else if (cameraGeneralBahviour._onRoomView == true)
        {
            trajectoryPredictor.enabled = false; //Si no view , no preview
            ResetProperties(); //Reset Values
        }
    }
    private void ManualAim()
    {
        
        if(Input.GetKey(KeyCode.RightArrow)) _predictedDirectionXYZ = Quaternion.AngleAxis(0.2f, Vector3.up) * _predictedDirectionXYZ; //Gauche-Droite
        if(Input.GetKey(KeyCode.LeftArrow)) _predictedDirectionXYZ = Quaternion.AngleAxis(-0.2f, Vector3.up) * _predictedDirectionXYZ; // 
        if(Input.GetKey(KeyCode.UpArrow)) _predictedDirectionXYZ = Quaternion.AngleAxis(0.2f, Vector3.Cross(_predictedDirectionXYZ,Vector3.up)) * _predictedDirectionXYZ; //Up-Down
        if(Input.GetKey(KeyCode.DownArrow)) _predictedDirectionXYZ = Quaternion.AngleAxis(-0.2f, Vector3.Cross(_predictedDirectionXYZ,Vector3.up)) * _predictedDirectionXYZ;
        _predictedDirectionXYZ.y = Mathf.Clamp(_predictedDirectionXYZ.y, 0f, 0.9f); // clamp pour eviter le lock vertical
        //Force Managment
        _shootForce = Mathf.Clamp(_shootForce, 0f, 15f);
        if(Input.GetKey(KeyCode.LeftControl)) _shootForce -= 0.05f;
        if(Input.GetKey(KeyCode.LeftShift)) _shootForce += 0.05f;
        Debug.Log(_shootForce);
        //Shoot
        if (Input.GetKeyDown(KeyCode.KeypadEnter)) _ballRigidbody.AddForce(_predictedDirectionXYZ.normalized * (_shootForce), ForceMode.Impulse );
    }

    private void ResetProperties()
    {
        _predictedDirectionXYZ = Vector3.forward;
        _shootForce = 0f;
        
    }
}
