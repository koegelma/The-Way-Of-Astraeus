using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float turnSpeed;
    [SerializeField] private float maxRollAngle;
    private float barrelRollAngle;
    private float barrelRollDirection;
    [SerializeField] private float barrelCooldownTime;
    [SerializeField] private Image circularCooldown;
    private float barrelCooldown = 0;
    private bool isBarrelRolling;
    private MeshCollider boundsColl;
    private Rigidbody rb;
    private Vector3 position;
    public float moveSpeed;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        boundsColl = Bounds.bounds.GetComponent<MeshCollider>();
        position = transform.position;
    }
    private void Update()
    {
        HndInput();
        UpdateCooldownUI();
    }

    private void FixedUpdate()
    {
        if (boundsColl.bounds.Contains(new Vector3(position.x, boundsColl.transform.position.y, position.z))) rb.MovePosition(position); // checks if position is within bounds (x,z) of groundPlane
    }

    private void HndInput()
    {
        float horizAxisVal = Input.GetAxisRaw("Horizontal");
        float vertAxisVal = Input.GetAxisRaw("Vertical");

        if (barrelCooldown <= 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && horizAxisVal != 0) // implement cooldown timer
            {
                isBarrelRolling = true;
                barrelRollAngle = 0;
                barrelRollDirection = horizAxisVal;
                barrelCooldown = barrelCooldownTime;
            }

        }
        barrelCooldown -= Time.deltaTime;



        if (isBarrelRolling)
        {
            BarrelRoll();
            return;
        }

        HndTranslation(new Vector2(horizAxisVal, vertAxisVal));
        HndRotation(horizAxisVal);
    }

    private void HndTranslation(Vector2 _direction)
    {
        _direction = Vector2.ClampMagnitude(_direction, 1);
        Vector3 movement = new Vector3(_direction.x, 0, _direction.y);
        position = rb.position + movement * moveSpeed * Time.fixedDeltaTime;
    }

    private void HndRotation(float _direction)
    {
        float rotation = transform.rotation.eulerAngles.z;
        if (rotation > 180) rotation -= 360;

        if (_direction == 0 && rotation != 0) // rotate back to 0 if idle
        {
            if (rotation < 0) rotation += turnSpeed * Time.deltaTime;
            if (rotation > 0) rotation -= turnSpeed * Time.deltaTime;
            if (rotation >= -1 && rotation <= 1) rotation = 0;
        }

        if (_direction < 0) rotation += turnSpeed * Time.deltaTime;
        if (_direction > 0) rotation -= turnSpeed * Time.deltaTime;

        if (_direction < 0 && rotation > maxRollAngle) rotation = maxRollAngle;
        if (_direction > 0 && rotation < -maxRollAngle) rotation = -maxRollAngle;

        transform.localRotation = Quaternion.Euler(0, 0, rotation);
    }

    private void BarrelRoll()
    {
        float barrelRollSpeed = moveSpeed * 12;
        float rotation = transform.rotation.eulerAngles.z;
        if (barrelRollDirection < 0) rotation += barrelRollSpeed * Time.deltaTime;
        if (barrelRollDirection > 0) rotation -= barrelRollSpeed * Time.deltaTime;

        transform.localRotation = Quaternion.Euler(0, 0, rotation);

        barrelRollAngle += barrelRollSpeed * Time.deltaTime;
        if (barrelRollAngle >= 360) isBarrelRolling = false;

        Vector3 movement = new Vector3(barrelRollDirection, 0, 0);
        position = rb.position + movement * (barrelRollSpeed / 8) * Time.fixedDeltaTime;
    }

    private void UpdateCooldownUI()
    {
        if (barrelCooldown <= 0) circularCooldown.fillAmount = 0;
        if (barrelCooldown > 0) circularCooldown.fillAmount = barrelCooldown / barrelCooldownTime;
    }

}
