using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform player;
    private PlayerMovement playerMovement;
    private float moveSpeed;

    private void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
        InvokeRepeating("GetPlayerMoveSpeed", 0, 1);
    }

    private void FixedUpdate()
    {
        Vector3 targetPosition = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private void GetPlayerMoveSpeed()
    {
        moveSpeed = playerMovement.moveSpeed * 0.15f;
    }
}
