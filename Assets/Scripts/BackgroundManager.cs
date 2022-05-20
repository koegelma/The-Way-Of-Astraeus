using System;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private GameObject backgroundPrefab;
    [SerializeField] private bool isMenu;
    [SerializeField] private Sprite[] blueSpritePrefabs;
    [SerializeField] private Sprite[] greenSpritePrefabs;
    [SerializeField] private Sprite[] purpleSpritePrefabs;
    [SerializeField] private Sprite[] starFieldSpritePrefabs;
    private GameObject[] backgrounds;
    private GameObject con;
    private int index;

    private void Start()
    {
        con = new GameObject("Backgrounds");
        backgrounds = new GameObject[3];
        index = backgrounds.Length - 1;
        Vector3 pos = transform.position;

        Sprite[] spritePrefabs = starFieldSpritePrefabs;

        int spriteIndex = UnityEngine.Random.Range(0, spritePrefabs.Length);

        if (isMenu)
        {
            spritePrefabs = starFieldSpritePrefabs;
            spriteIndex = 0;
        }

        for (int i = 0; i < 3; i++)
        {
            backgrounds[i] = Instantiate(backgroundPrefab, pos, Quaternion.identity);

            SpriteRenderer[] sprites = backgrounds[i].GetComponent<Background>().sprites;
            foreach (SpriteRenderer rend in sprites)
            {
                rend.sprite = spritePrefabs[spriteIndex];
            }

            pos.z -= 205;
            backgrounds[i].transform.SetParent(con.transform);
        }
    }

    private void Update()
    {
        con.transform.position += Vector3.back * 20 * Time.deltaTime;

        if (backgrounds[index].transform.position.z <= -365f)
        {
            Vector3 newPosition = backgrounds[GetTopIndex()].GetComponent<Background>().dockingPoint.position;
            backgrounds[index].transform.position = newPosition;
            index = GetNextIndex();
        }
    }

    private int GetTopIndex()
    {
        int topBackGround = index + 1;
        if (topBackGround >= backgrounds.Length) topBackGround = 0;
        return topBackGround;
    }

    private int GetNextIndex()
    {
        int nextIndex = index - 1;
        if (nextIndex < 0) nextIndex = backgrounds.Length - 1;
        return nextIndex;
    }
}
