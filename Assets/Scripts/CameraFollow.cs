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
        boundsColl = CameraBounds.bounds.GetComponent<MeshCollider>();
        InvokeRepeating("GetPlayerMoveSpeed", 0, 1); // maybe change to start of location, if it's not possible to change speed during location
        newSpeed = moveSpeed / 10;
    }

    private void FixedUpdate()
    {
        float currSpeed = moveSpeed;
        Vector3 targetPosition = player.position + offset;
        /* if (!boundsColl.bounds.Contains(new Vector3(targetPosition.x, boundsColl.transform.position.y, targetPosition.z)))
        {
            t += 0.1f;
            currSpeed = Mathf.Lerp(currSpeed, newSpeed, t);
            //currMoveSpeed /= 5;
        }
        else
        {
            t = 0;
            currSpeed = moveSpeed;
        } */
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
