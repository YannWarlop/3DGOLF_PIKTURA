using System;
using UnityEngine;
public class TrajectoryPredictor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] LineRenderer trajectoryLine;
    [SerializeField] Transform hitMarker; //Marker de l'impact
    [Header("Properties")]
    [SerializeField, Range(10, 100)] int maxPoints = 50; //Nombre de points de la Prediction
    [SerializeField, Range(0.01f, 0.5f)] float increment = 0.025f; //Increment Temporel du calcul de trajectoire
    [SerializeField, Range(1.05f, 2f)] float rayOverlap = 1.1f; //Overlap entre els points de la ligne

    private void OnEnable()
    {
        SetTrajectoryVisible(true);
        Physics.queriesHitTriggers = false;
    }

    public void PredictTrajectory(BallProperties projectile)
    {
        Vector3 velocity =  projectile.direction * (projectile.initialForce / projectile.mass);
        Vector3 position = projectile.initialPosition;
        Vector3 nextPosition;
        float overlap;

        UpdateLineRender(maxPoints, (0, position));

        for (int i = 1; i < maxPoints; i++)
        {
            // Estimate velocity and update next predicted position
            velocity = CalculateNewVelocity(velocity, projectile.drag, increment);
            nextPosition = position + velocity * increment;

            // Overlap our rays by small margin to ensure we never miss a surface
            overlap = Vector3.Distance(position, nextPosition) * rayOverlap;

            //When hitting a surface we want to show the surface marker and stop updating our line
            if (Physics.Raycast(position, velocity.normalized, out RaycastHit hit, overlap))
            {
                UpdateLineRender(i, (i - 1, hit.point));
                MoveHitMarker(hit);
                break;
            }

            //If nothing is hit, continue rendering the arc without a visual marker
            hitMarker.gameObject.SetActive(false);
            position = nextPosition;
            UpdateLineRender(maxPoints, (i, position)); //Unneccesary to set count here, but not harmful
        }
    }
    private void UpdateLineRender(int count, (int point, Vector3 pos) pointPos)
    {
        trajectoryLine.positionCount = count;
        trajectoryLine.SetPosition(pointPos.point, pointPos.pos);
    }

    private Vector3 CalculateNewVelocity(Vector3 velocity, float drag, float increment)
    {
        velocity += Physics.gravity * increment;
        velocity *= Mathf.Clamp01(1f - drag * increment);
        return velocity;
    }

    private void MoveHitMarker(RaycastHit hit)
    {
        hitMarker.gameObject.SetActive(true);

        // Offset marker from surface
        float offset = 0.025f;
        hitMarker.position = hit.point + hit.normal * offset;
        hitMarker.rotation = Quaternion.LookRotation(hit.normal, Vector3.up);
    }

    public void SetTrajectoryVisible(bool visible)
    {
        trajectoryLine.enabled = visible;
        hitMarker.gameObject.SetActive(visible);
    }
}
