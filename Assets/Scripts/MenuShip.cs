using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuShip : MonoBehaviour
{
    [SerializeField] private float turnSpeed;
    [SerializeField] private float maxRollAngle;
    private Rigidbody rb;
    private float rollDirection;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rollDirection = -1;
    }

    private void Update()
    {
        BarrelRoll();
    }

    private void BarrelRoll()
    {
        float rotation = transform.rotation.eulerAngles.z;
        if (rotation > 180) rotation -= 360;
        if (rollDirection < 0)
        {
            rotation += turnSpeed * Time.deltaTime;
            if (rotation > maxRollAngle)
            {
                rotation = maxRollAngle;
                rollDirection = -rollDirection;
            }
        }
        if (rollDirection > 0)
        {
            rotation -= turnSpeed * Time.deltaTime;
            if (rotation < -maxRollAngle)
            {
                rotation = -maxRollAngle;
                rollDirection = -rollDirection;
            }
        }
        transform.localRotation = Quaternion.Euler(0, 0, rotation);
    }
}
