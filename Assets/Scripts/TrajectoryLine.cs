using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A large part of the code in this script is taken from the following source: https://www.youtube.com/watch?v=RpeRnlLgmv8&ab_channel=MuddyWolf
public class TrajectoryLine : MonoBehaviour
{
    [Header("Trajectory Line Settings")]
    [SerializeField, Tooltip("Smoothness of the line")]
    private int _segmentCount = 100;

    private LineRenderer _lineRenderer;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void PlotTrajectoryLine(Rigidbody2D rigidbody, Vector2 pos, float throwSpeed)
    {
        Vector2 velocity = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * throwSpeed * 3f;

        Vector2[] trajectory = CalculateTrajectoryLine(rigidbody, pos, velocity, _segmentCount);
        
        _lineRenderer.positionCount = trajectory.Length;

        // convert Vector2 array to Vector3 array
        Vector3[] positions = new Vector3[trajectory.Length];
        for (int i = 0; i < trajectory.Length; i++)
        {
            positions[i] = trajectory[i];
        }

        // Write positions to line renderer
        _lineRenderer.SetPositions(positions);
    }

    public void ClearTrajectoryLine()
    {
        _lineRenderer.positionCount = 0;
    }

    private Vector2[] CalculateTrajectoryLine(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity, int steps)
    {
        Vector2[] results = new Vector2[steps];

        float timestep = Time.fixedDeltaTime / Physics2D.velocityIterations;
        Vector2 gravityAccel = Physics2D.gravity * rigidbody.gravityScale * timestep * timestep;

        float drag = 1f - timestep * rigidbody.drag;
        Vector2 moveStep = velocity * timestep;

        for (int i = 0; i < steps; i++)
        {
            moveStep += gravityAccel;
            moveStep *= drag;
            pos += moveStep;
            results[i] = pos;
        }

        return results;
    }
}
