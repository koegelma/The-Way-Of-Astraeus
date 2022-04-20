using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject groundPlane;
    private MeshCollider groundColl;
    private Rigidbody rb;
    private Vector3 position = Vector3.zero;
    public float moveSpeed;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        groundColl = groundPlane.GetComponent<MeshCollider>();
    }
    private void Update()
    {
        HndInput();
    }

    private void FixedUpdate()
    {
        if (groundColl.bounds.Contains(new Vector3(position.x, 0, position.z))) rb.MovePosition(position); // checks if position is within bounds of groundPlane
    }

    private void HndInput()
    {
        Vector2 inputVec2D = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputVec2D = Vector2.ClampMagnitude(inputVec2D, 1); // clamps magnitude of vector to 1, so that diagonal movement isn't faster
        Vector3 movement = new Vector3(inputVec2D.x, 0, inputVec2D.y);
        position = rb.position + movement * moveSpeed * Time.fixedDeltaTime;
    }

}
