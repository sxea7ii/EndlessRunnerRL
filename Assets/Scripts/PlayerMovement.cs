using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float forwardSpeed = 5f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Move player forward smoothly using physics
        Vector3 newPos = rb.position + Vector3.forward * forwardSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }
}
