using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float mouseZPos;
    private Vector3 offset;
    private Vector3 targetPosition;
    private bool moveToTarget;

    public float speed;

    private void Update()
    {
        HndInput();
        if (moveToTarget) MoveToTarget();
    }

    private void HndInput()
    {
        if (Input.GetKey(KeyCode.W)) transform.Translate(Vector3.forward * Time.deltaTime * speed);
        if (Input.GetKey(KeyCode.S)) transform.Translate(Vector3.back * Time.deltaTime * speed);
        if (Input.GetKey(KeyCode.A)) transform.Translate(Vector3.left * Time.deltaTime * speed);
        if (Input.GetKey(KeyCode.D)) transform.Translate(Vector3.right * Time.deltaTime * speed);

        if (Input.GetMouseButton(0)) HndMouseInput();
    }

    private void HndMouseInput()
    {
        Ray ray = Camera.allCameras[0].ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            moveToTarget = true;
        }
    }

    private void MoveToTarget()
    {
        if (Vector3.Distance(transform.position, targetPosition) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
            return;
        }
        transform.position = targetPosition;
        moveToTarget = false;
    }

    // if player attacks
    public void CancelMovement()
    {
        moveToTarget = false;
    }
}
