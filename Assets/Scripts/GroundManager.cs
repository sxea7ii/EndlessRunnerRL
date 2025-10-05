using UnityEngine;

public class GroundManager : MonoBehaviour
{
    public Transform[] grounds;       // Assign all ground pieces in order
    public Transform player;          // Assign Player capsule here
    public float groundLength = 20f;  // Length of one ground piece
    public float recycleOffset = 10f; // Distance from player to recycle

    void Update()
    {
        foreach (Transform ground in grounds)
        {
            if (player.position.z - ground.position.z > groundLength)
            {
                float newZ = ground.position.z + grounds.Length * groundLength;
                ground.position = new Vector3(ground.position.x, ground.position.y, newZ);
            }
        }
    }
}

