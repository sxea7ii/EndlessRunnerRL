using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    public float speed = 5f; // Speed of obstacles toward the player

    void Update()
    {
        // Move the obstacle backward along Z
        transform.Translate(Vector3.back * speed * Time.deltaTime);

        // Destroy the obstacle if it goes past the player
        if (transform.position.z < -10f)
        {
            Destroy(gameObject);
        }
    }
}

