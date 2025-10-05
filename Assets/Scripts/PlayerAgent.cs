using UnityEngine;

public class PlayerAgent : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right arrow
        transform.Translate(Vector3.right * horizontal * moveSpeed * Time.deltaTime);
    }
}
