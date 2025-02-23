using UnityEngine;
using System;

public class BallPhysicsBehaviour : MonoBehaviour {
    
    [Header("REFERENCES")]
    public Rigidbody rb;
    public LineRenderer lr;
    public Camera cam;
    [Header("Attributes")]
    public float maxpower = 10f;
    public float powermult = 5f;
    
    
    private bool _isAiming;
    private float iniPos = 0f;
    private float currPos = 0f;
    private float distance = 0f;
    private void Update()
    {
        if (rb.velocity.magnitude < 0.2f) // Si immobile
        {
            if (Input.GetMouseButtonDown(0)) //AimStart
            {
                iniPos = Input.mousePosition.y;
                Debug.Log("Aiming Start");
                _isAiming = true;
                lr.positionCount = 2;
            }
            if (Input.GetMouseButton(0) && _isAiming) //AimCurrent
            {
                currPos = Input.mousePosition.y;
                distance = iniPos - currPos;
                Debug.Log("Aiming, distance :" + distance);
                lr.SetPosition(0, transform.position);
                Vector3 direction = new Vector3(transform.position.x - Camera.main.transform.position.x, transform.position.y,
                    transform.position.z - Camera.main.transform.position.z);
                lr.SetPosition(1,
                    new Vector3(transform.position.x + direction.x, transform.position.y,
                        transform.position.z + direction.z).normalized * distance);
            }
    
            if (Input.GetMouseButtonUp(0) && distance > 1f )//Aimrelease si minimum distance
            {
                Debug.Log("Aiming End");
                lr.positionCount = 0;
                _isAiming = false;
                Vector3 direction = new Vector3(transform.position.x - Camera.main.transform.position.x, 0,transform.position.z - Camera.main.transform.position.z ).normalized;
                rb.AddForce(direction * distance * powermult);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Finish")
        {
            if (rb.velocity.magnitude < 0.2f) Debug.Log("Finish");
        }
    }
    
}
