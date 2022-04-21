using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject groundPlane;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float maxRollAngle;
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
        if (groundColl.bounds.Contains(new Vector3(position.x, groundColl.transform.position.y, position.z))) rb.MovePosition(position); // checks if position is within bounds (x,z) of groundPlane
    }

    private void HndInput()
    {
        // Translation
        Vector2 inputVec2D = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputVec2D = Vector2.ClampMagnitude(inputVec2D, 1); // clamps magnitude of vector to 1, so that diagonal movement isn't faster
        Vector3 movement = new Vector3(inputVec2D.x, 0, inputVec2D.y);
        position = rb.position + movement * moveSpeed * Time.fixedDeltaTime;

        // Rotation
        // TODO: sometimes buggy when switching rapidly between left/right
        float roll = turnSpeed * Time.deltaTime * Input.GetAxisRaw("Horizontal") * -1;
        if (roll == 0)
        {
            if (transform.rotation.z > -0.005f && transform.rotation.z < 0.005f)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                return;
            }
            float rotation = turnSpeed * Time.deltaTime * transform.rotation.z * -5;
            transform.Rotate(0, 0, rotation);
            return;
        }
        if (transform.rotation.eulerAngles.z > maxRollAngle && transform.rotation.eulerAngles.z < 360 - maxRollAngle) return;
        transform.Rotate(0, 0, roll);
    }

}
