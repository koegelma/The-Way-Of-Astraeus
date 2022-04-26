using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] backgrounds;
    [SerializeField] private GameObject[] backgroundLines;
    [SerializeField] private GameObject backgroundContainerPrefab;
    public GameObject backgroundContainer;
    [SerializeField] private Transform start;
    private ObjectPooler objectPooler;

    private void Start()
    {
        objectPooler = ObjectPooler.instance;
        objectPooler.CreatePoolFromArray("backgroundLines", backgroundLines, 6);

        Vector3 currPos = start.position;

        for (int y = 0; y < 18; y++)
        {
            backgroundContainer = objectPooler.SpawnFromPool("backgroundLines", currPos, Quaternion.identity);
            currPos.z -= 16;
        }
    }

    private void Update()
    {
        if (backgroundContainer.transform.position.z < 124) SpawnNewLine();
    }

    private void SpawnNewLine()
    {
        Vector3 currPos = new Vector3(backgroundContainer.transform.position.x, backgroundContainer.transform.position.y, backgroundContainer.transform.position.z + 15.5f);
        backgroundContainer = objectPooler.SpawnFromPool("backgroundLines", currPos, Quaternion.identity);
    }
}
