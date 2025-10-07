using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.UI;
using TMPro;

public class PlayerAgent : Agent
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float laneLimit = 4f;

    [Header("UI")]
    public TMP_Text stepRewardText;
    public TMP_Text totalRewardText;


    private Rigidbody rb;
    private float lastStepReward = 0f;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        // Reset player position
        transform.localPosition = new Vector3(0f, 0.5f, 0f);
        rb.velocity = Vector3.zero;

        // Destroy all obstacles in the scene
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            Destroy(obj);
        }

        lastStepReward = 0f;
        UpdateRewardUI();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Observe player x position and x velocity
        sensor.AddObservation(transform.localPosition.x);
        sensor.AddObservation(rb.velocity.x);

        // Optionally, add nearest obstacle x distance
        GameObject nearest = FindNearestObstacle();
        if (nearest != null)
        {
            sensor.AddObservation(nearest.transform.localPosition.x - transform.localPosition.x);
            sensor.AddObservation(nearest.transform.localPosition.z - transform.localPosition.z);
        }
        else
        {
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Continuous action for horizontal movement
        float moveX = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
        Vector3 move = new Vector3(moveX * moveSpeed * Time.deltaTime, 0f, 0f);
        transform.Translate(move, Space.World);

        // Clamp within lane limits
        float clampedX = Mathf.Clamp(transform.localPosition.x, -laneLimit, laneLimit);
        transform.localPosition = new Vector3(clampedX, transform.localPosition.y, transform.localPosition.z);

        // Step reward
        lastStepReward = 0.01f;
        AddReward(lastStepReward);

        UpdateRewardUI();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetKey(KeyCode.LeftArrow) ? -1f :
                                  Input.GetKey(KeyCode.RightArrow) ? 1f : 0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            SetReward(-1f);
            EndEpisode();
            Debug.Log("Hit obstacle! Episode ended.");
        }
    }

    private GameObject FindNearestObstacle()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        GameObject nearest = null;
        float minDistance = float.MaxValue;

        foreach (GameObject obs in obstacles)
        {
            float distance = obs.transform.localPosition.z - transform.localPosition.z;
            if (distance >= 0 && distance < minDistance)
            {
                minDistance = distance;
                nearest = obs;
            }
        }

        return nearest;
    }

    private void UpdateRewardUI()
    {
        if (stepRewardText != null)
            stepRewardText.text = "Step Reward: " + lastStepReward.ToString("F2");
        if (totalRewardText != null)
            totalRewardText.text = "Total Reward: " + GetCumulativeReward().ToString("F2");
    }
}













