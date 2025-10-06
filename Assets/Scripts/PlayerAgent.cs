using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class PlayerAgent : Agent
{
    public float moveSpeed = 8f;
    public float laneLimit = 4f;
    private Rigidbody rb;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        // Reset position
        transform.localPosition = new Vector3(0, 0.5f, 0);

        // Destroy existing obstacles
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            Destroy(obj);
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // X position and velocity
        sensor.AddObservation(transform.localPosition.x);
        sensor.AddObservation(rb.velocity.x);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
        Vector3 move = new Vector3(moveX * moveSpeed * Time.deltaTime, 0, 0);
        transform.Translate(move, Space.World);

        // Keep within lane limits
        float clampedX = Mathf.Clamp(transform.localPosition.x, -laneLimit, laneLimit);
        transform.localPosition = new Vector3(clampedX, transform.localPosition.y, transform.localPosition.z);

        AddReward(0.01f); // Reward for staying alive
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            SetReward(-1f);
            EndEpisode();
            Debug.Log("Game Over! Restarting episode...");
        }
    }
}





