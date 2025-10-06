using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.SceneManagement;

public class PlayerAgent : Agent
{
    public float moveSpeed = 5f;
    public float laneWidth = 2f;
    private Rigidbody rb;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        // Reset position
        transform.localPosition = new Vector3(0, 0.5f, 0);

        // Reset obstacles and scene
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            Destroy(obj);
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Observe player X position and velocity
        sensor.AddObservation(transform.localPosition.x);
        sensor.AddObservation(rb.velocity.x);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
        Vector3 move = new Vector3(moveX, 0, 0) * moveSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);

        AddReward(0.01f); // small reward for staying alive
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
