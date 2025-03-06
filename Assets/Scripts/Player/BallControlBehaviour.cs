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
    
    [Header("References - FX")]
    [SerializeField] private GameObject _impactFX;
    
    [Header("Attributes")] 
    [SerializeField] [Range(0f, 5f)] private float _aimSensitivity = 1f; //Sensi Multiplier de visée
    private CinemachineFreeLook _vcCinemachineCamera;
    private Rigidbody _ballRigidbody;
    
    // Shoot Related
    private Vector3 _predictedDirectionXYZ;
    private Vector2 _mouseAim = new Vector2(0,0);
    private float _shootForce = 2.5f;
    void Start()
    {
        //Setup
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
            //Power
            MousePower();
            //Shoot
            if (Input.GetMouseButtonDown(0) && _shootForce > 0.1f) {
                _ballRigidbody.AddForce(_predictedDirectionXYZ.normalized * (_shootForce), ForceMode.Impulse); //Si au moins 1 farce, on peut shoot
                trajectoryPredictor.SetTrajectoryVisible(false);
                trajectoryPredictor.enabled = false; //On shoot disable preview
            }
            //View
            if (Input.GetMouseButton(1)) {
                //Set sur input Souris
                _vcCinemachineCamera.m_XAxis.m_InputAxisName = "Mouse X";
                _vcCinemachineCamera.m_YAxis.m_InputAxisName = "Mouse Y";
                }
            else if (Input.GetMouseButtonUp(1)) {
                //Unset sur input Souris
                _vcCinemachineCamera.m_XAxis.m_InputAxisName = "";
                _vcCinemachineCamera.m_YAxis.m_InputAxisName = "";
            }
            if (Input.GetMouseButton(2)) MouseAim(); //Held Mouse Aim
            
            if (_ballRigidbody.velocity.magnitude < 0.01f) //SSI BALLE IMMOBILE - Predicition + Shoot;
            {
                trajectoryPredictor.SetTrajectoryVisible(true);
                _lineTrajectory.enabled = true;
                
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
            } 
            else {
                trajectoryPredictor.SetTrajectoryVisible(false);
                trajectoryPredictor.enabled = false; //Si mouvement , no preview
                ResetProperties(); //Reset Values
            }
        }
        else if (cameraGeneralBahviour._onRoomView == true)
        {
            //Reset Shoot quand switch de camera
            //trajectoryPredictor.enabled = false; //Si no view , no preview
            //ResetProperties(); //Reset Values
        }
    }
    private void MouseAim()
    {
        _mouseAim.x = Input.GetAxis("Mouse X") * (_aimSensitivity * 300) * Time.deltaTime; //SensScaleUp -> NormaliseTemp
        _mouseAim.y = Input.GetAxis("Mouse Y") * (_aimSensitivity * 300) * Time.deltaTime;
        
        _predictedDirectionXYZ = Quaternion.AngleAxis(_mouseAim.x, Vector3.up) * _predictedDirectionXYZ; //Gauche-Droite
        _predictedDirectionXYZ = Quaternion.AngleAxis(_mouseAim.y, Vector3.Cross(_predictedDirectionXYZ, Vector3.up)) * _predictedDirectionXYZ;//Up-Down
        
        //_predictedDirectionXYZ.y = Mathf.Clamp(_predictedDirectionXYZ.y, 0.1f, 0.9f); // clamp pour eviter le lock vertical
        //_predictedDirectionXYZ.x = Mathf.Clamp(_predictedDirectionXYZ.x, 0.1f, 0.9f);
        //_predictedDirectionXYZ.z = Mathf.Clamp(_predictedDirectionXYZ.z, 0.1f, 0.9f);
    }

    private void MousePower()
    {
        //Force Managment
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) _shootForce += 0.2f; //ScrollAvant
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) _shootForce -= 0.2f; //ScrollArrière
        _shootForce = Mathf.Clamp(_shootForce, 0f, 20f);
    }

    private void ResetProperties()
    {
        _predictedDirectionXYZ = Vector3.forward;
        _shootForce = 2.5f;
    }

    private void OnCollisionEnter(Collision col) {
        Instantiate(_impactFX,gameObject.transform.position,Quaternion.identity);
    }
}
