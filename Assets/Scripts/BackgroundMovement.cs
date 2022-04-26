using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    void Update()
    {
        if (transform.position.z < -140) gameObject.SetActive(false);

        transform.position += Vector3.back * 20 * Time.deltaTime;
    }
}
