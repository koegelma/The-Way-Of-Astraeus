using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    public static Transform bounds;

    private void Start()
    {
        bounds = transform;
    }
}
