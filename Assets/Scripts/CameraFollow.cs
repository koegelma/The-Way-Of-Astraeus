using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform player;
    private PlayerMovement playerMovement;
    private float moveSpeed;
    private float currSpeed;
    private float newSpeed;
    private float t = 0;
    private MeshCollider boundsColl;

    private void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
        InvokeRepeating("GetPlayerMoveSpeed", 0, 1); // change to get playerspeed only at start of location, if it's not possible to change speed during location (via pickup)
        newSpeed = moveSpeed / 10;
        transform.position = player.position + offset;
    }

    private void FixedUpdate()
    {
        float currSpeed = moveSpeed;
        Vector3 targetPosition = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, currSpeed * Time.deltaTime);
    }

    private void UpdateSpeed()
    {
        currSpeed = Mathf.Lerp(currSpeed, newSpeed, t);
    }

    private void GetPlayerMoveSpeed()
    {
        moveSpeed = playerMovement.moveSpeed * 0.15f;
    }
}
