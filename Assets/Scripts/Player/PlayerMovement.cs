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
        float horizAxisVal = Input.GetAxisRaw("Horizontal");
        float vertAxisVal = Input.GetAxisRaw("Vertical");
        
        // Translation
        Vector2 inputVec2D = new Vector2(horizAxisVal, vertAxisVal);
        inputVec2D = Vector2.ClampMagnitude(inputVec2D, 1);
        Vector3 movement = new Vector3(inputVec2D.x, 0, inputVec2D.y);
        position = rb.position + movement * moveSpeed * Time.fixedDeltaTime;

        // Rotation
        float rotation = transform.rotation.eulerAngles.z;
        if (rotation > 180) rotation -= 360;

        if (horizAxisVal == 0 && rotation != 0) // rotate back to 0 if idle
        {
            if (rotation < 0) rotation += turnSpeed * Time.deltaTime;
            if (rotation > 0) rotation -= turnSpeed * Time.deltaTime;
            if (rotation >= -1 && rotation <= 1) rotation = 0;
        }

        if (horizAxisVal < 0) rotation += turnSpeed * Time.deltaTime;
        if (horizAxisVal > 0) rotation -= turnSpeed * Time.deltaTime;

        if (horizAxisVal < 0 && rotation > maxRollAngle) rotation = maxRollAngle;
        if (horizAxisVal > 0 && rotation < -maxRollAngle) rotation = -maxRollAngle;

        transform.localRotation = Quaternion.Euler(0, 0, rotation);
    }
}
