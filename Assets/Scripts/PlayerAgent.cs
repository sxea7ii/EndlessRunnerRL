using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAgent : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float moveRange = 5f;

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        Vector3 newPosition = transform.position + new Vector3(moveX * moveSpeed * Time.deltaTime, 0, 0);
        newPosition.x = Mathf.Clamp(newPosition.x, -moveRange, moveRange);
        transform.position = newPosition;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over! Restarting…");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

