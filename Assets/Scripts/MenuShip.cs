using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuShip : MonoBehaviour
{
    [SerializeField] private float turnSpeed;
    [SerializeField] private float maxRollAngle;
    private Rigidbody rb;
    private float rollDirection;
    private Vector3 targetPosition;
    public bool newGameStarting = false;
    public bool shipIsInPosition = false;


    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rollDirection = -1;
        targetPosition = new Vector3(0, 0, -20);
    }

    private void Update()
    {
        /* if (newGameStarting)
        {
            ResetRotation();
            TranslateToTarget();
            if (transform.rotation.eulerAngles.z == 0 && transform.position == targetPosition) shipIsInPosition = true;
            return;
        } */

        BarrelRoll();
    }

    private void ResetRotation()
    {
        float rotation = transform.rotation.eulerAngles.z;
        if (rotation > 180) rotation -= 360;
        if (rotation < 0) rotation += turnSpeed * 5 * Time.deltaTime;
        if (rotation > 0) rotation -= turnSpeed * 5 * Time.deltaTime;
        if (rotation >= -1 && rotation <= 1) rotation = 0;
        transform.localRotation = Quaternion.Euler(0, 0, rotation);
    }

    private void TranslateToTarget()
    {
        if (Vector3.Distance(transform.position, targetPosition) <= 1)
        {
            transform.position = targetPosition;
            return;
        }
        Vector3 direction = targetPosition - transform.position;
        transform.Translate(direction * 4 * Time.deltaTime);
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
