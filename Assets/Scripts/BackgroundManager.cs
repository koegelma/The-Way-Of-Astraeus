using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private GameObject backgroundPrefab;
    [SerializeField] private Sprite[] spritePrefabs;
    private GameObject[] backgrounds;
    private GameObject con;
    private int index;

    private void Start()
    {
        con = new GameObject("Backgrounds"); //

        backgrounds = new GameObject[3];
        index = backgrounds.Length - 1;
        Vector3 pos = transform.position;

        int spriteIndex = Random.Range(0, spritePrefabs.Length);

        for (int i = 0; i < 3; i++)
        {
            backgrounds[i] = Instantiate(backgroundPrefab, pos, Quaternion.identity);

            SpriteRenderer[] sprites = backgrounds[i].GetComponent<Background>().sprites;
            foreach (SpriteRenderer rend in sprites)
            {
                rend.sprite = spritePrefabs[spriteIndex];
            }

            pos.z -= 205;
            backgrounds[i].transform.SetParent(con.transform); //
        }
    }

    private void Update()
    {
        con.transform.position += Vector3.back * 20 * Time.deltaTime; //

        if (backgrounds[index].transform.position.z <= -307.5f)
        {
            Vector3 newPosition = backgrounds[GetTopBackground()].GetComponent<Background>().dockingPoint.position;
            backgrounds[index].transform.position = newPosition;
            index = GetNextIndex();
        }
    }

    private int GetTopBackground()
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
