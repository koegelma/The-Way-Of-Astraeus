using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform player;
    private float moveSpeed;

    private void FixedUpdate()
    {
        moveSpeed = player.GetComponent<PlayerMovement>().moveSpeed * 0.15f;
        Vector3 targetPosition = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
