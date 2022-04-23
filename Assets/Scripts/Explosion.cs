using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<ParticleSystem>().Play();
    }

    private void Update()
    {
        if (GetComponent<ParticleSystem>().isStopped) gameObject.SetActive(false);
    }
}
