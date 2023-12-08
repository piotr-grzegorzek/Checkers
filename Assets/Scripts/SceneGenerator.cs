using UnityEngine;

public class SceneGenerator : MonoBehaviour
{
    public int BoardSize = 8;
    public GameObject DarkTilePrefab;
    public GameObject LightTilePrefab;
    public GameObject DarkPawnPrefab;
    public GameObject LightPawnPrefab;

    void Start()
    {
        for (int x = 0; x < BoardSize; x++)
        {
            for (int z = 0; z < BoardSize; z++)
            {
                if ((x + z) % 2 == 0)
                {
                    Instantiate(DarkTilePrefab, new Vector3(x, 0, z), Quaternion.identity);
                    if (z < 3)
                    {
                        Instantiate(LightPawnPrefab, new Vector3(x, 0.5f, z), Quaternion.identity);
                    }
                    else if (z > 4)
                    {
                        Instantiate(DarkPawnPrefab, new Vector3(x, 0.5f, z), Quaternion.identity);
                    }
                }
                else
                {
                    Instantiate(LightTilePrefab, new Vector3(x, 0, z), Quaternion.identity);
                }
            }
        }
    }
    void Update()
    {

    }
}
